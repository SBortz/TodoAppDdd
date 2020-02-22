using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using Newtonsoft.Json;
using TodoAppDdd.Domain.Aggregate;
using TodoAppDdd.Domain.DDDBase;
using TodoAppDdd.Domain.Event;
using TodoAppDdd.Domain.ReadModel;
using TodoAppDdd.Persistence.EventStore;

namespace TodoAppDdd.Persistence
{
    public class TodoRepository : ITodoRepository
	{
		private readonly IEventStore _eventStore;
        private IEventStoreConnection _conn;

        public TodoRepository(IEventStore eventStore, IEventStoreConnection eventStoreConnection)
        {
            this._conn = eventStoreConnection;
            this._eventStore = eventStore;
        }

        public async Task<TodoItem> GetTodoAsync(string todoItemId)
        {
            var streamEvents = new List<ResolvedEvent>();

            StreamEventsSlice currentSlice;
            long nextSliceStart = StreamPosition.Start;
            do
            {
                currentSlice = await this._conn.ReadStreamEventsForwardAsync(todoItemId, nextSliceStart, 200, false);

                nextSliceStart = currentSlice.NextEventNumber;

                streamEvents.AddRange(currentSlice.Events);
            } while (!currentSlice.IsEndOfStream);

            var domainEvents = new List<IDomainEvent>();
            foreach (var resolvedEvent in streamEvents)
            {
                var domainEventJson = this.ByteArrayToString(resolvedEvent.Event.Data);
                Type eventType = null;
                switch (resolvedEvent.Event.EventType)
                {
                    case nameof(TodoItemCreated):
                        eventType = typeof(TodoItemCreated);
                        break;
                    case nameof(TodoItemDiscarded):
                        eventType = typeof(TodoItemDiscarded);
                        break;
                    case nameof(TodoItemMarkedAsFinished):
                        eventType = typeof(TodoItemMarkedAsFinished);
                        break;
                    case nameof(TodoItemOrderUpdated):
                        eventType = typeof(TodoItemOrderUpdated);
                        break;
                    case nameof(TodoItemResetted):
                        eventType = typeof(TodoItemResetted);
                        break;
                    case nameof(TodoItemRestored):
                        eventType = typeof(TodoItemRestored);
                        break;
                    case nameof(TodoItemTitleUpdated):
                        eventType = typeof(TodoItemTitleUpdated);
                        break;
                }
                var domainEvent = JsonConvert.DeserializeObject(domainEventJson, eventType);
                
                domainEvents.Add((IDomainEvent)domainEvent);
            }
            TodoItem todoItem = new TodoItem(domainEvents);

            return todoItem;
        }

        public async Task<IEnumerable<TodoItem>> GetAllTodos(int? goBackMinutes = null)
        {
            await this._conn.ReadEventAsync("all-todoitems", StreamPosition.End, false);
            
            var todoList = new List<TodoItem>();
            var lastEvent = await this._conn.ReadEventAsync("all-todoitems", StreamPosition.End, false);
            if (lastEvent != null && lastEvent.Event.HasValue)
            {
                var json = this.ByteArrayToString(lastEvent.Event.Value.Event.Data);
                var allTodoItems = JsonConvert.DeserializeObject<AllTodoItems>(json);

                foreach (var todoItemId in allTodoItems.TodoItems)
                {
                    var todoItem = await this.GetTodoAsync(todoItemId);
                    todoList.Add(todoItem);
                }
            }

            return todoList;
        }

		public async Task<IEnumerable<TodoItem>> GetLastDiscardedTodos()
		{
			var discardedEvents = await this._eventStore.GetAll<TodoItemDiscarded>();
			var lastEvents = discardedEvents
				.OrderByDescending(x => x.CreatedOn);
				
			var todos = new List<TodoItem>();
			foreach (var todoItemDiscarded in lastEvents)
			{
				var todo = await this.GetTodoAsync(todoItemDiscarded.Id);

				if (todo.IsDiscarded)
				{
					todos.Add(todo);
				}
				
				if (todos.Count >= 1)
				{
					return todos;
				}
			}

			return todos;
		}

		public async Task SaveState(TodoItem todoItem)
		{
			await this._eventStore.AppendEvents(todoItem.DomainEvents);
		}

        private byte[] StringToByteArray(string str)
        {
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            return enc.GetBytes(str);
        }

        private string ByteArrayToString(byte[] arr)
        {
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            return enc.GetString(arr);
        }
    }
}