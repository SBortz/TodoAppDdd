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
using TodoAppDdd.Persistence.EventStore;

namespace TodoAppDdd.Persistence
{
	public class TodoRepository : ITodoRepository
	{
		private readonly IEventStore _eventStore;
        private readonly IEventStoreConnection betterEventStore;

        private IEventStoreConnection _conn;
        private UserCredentials userCredentials;

        public TodoRepository(IEventStore eventStore)
        {
            this._eventStore = eventStore;
        }

        private async Task EnsureEventStoreConnection()
        {
            if (this._conn != null)
            {
                return;
            }

            this.userCredentials = new UserCredentials("admin", "changeit");
            this._conn = EventStoreConnection.Create(new Uri("tcp://admin:changeit@localhost:1113"),
                "InputFromFileConsoleApp");

            await this._conn.ConnectAsync();
        }

        public async Task<TodoItem> GetTodoAsync(string todoItemId)
        {
            await this.EnsureEventStoreConnection();

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
            await this.EnsureEventStoreConnection();

            var streamEvents = new List<ResolvedEvent>();

            AllEventsSlice currentSlice;
            var nextSliceStart = Position.Start;
            do
            {
                currentSlice = await this._conn.ReadAllEventsForwardAsync(nextSliceStart, 200, false, this.userCredentials);

                nextSliceStart = currentSlice.NextPosition;
                Debug.WriteLine($"Reading all events FromPosition: {currentSlice.FromPosition}, NextPosition: {currentSlice.NextPosition}, EventCount: {currentSlice.Events.Length}");

                streamEvents.AddRange(currentSlice.Events.Where(x => x.Event.EventType == nameof(TodoItemCreated)));
            } while (!currentSlice.IsEndOfStream);

            var todoItemCreatedEvents = new List<TodoItemCreated>();
            Debug.WriteLine($"StreamEvents.Count(): {streamEvents.Count}");
            foreach (var resolvedEvent in streamEvents)
            {
                var json = this.ByteArrayToString(resolvedEvent.Event.Data);
                var todoItemCreatedEvent = JsonConvert.DeserializeObject<TodoItemCreated>(json);

                todoItemCreatedEvents.Add(todoItemCreatedEvent);
            }

            var todoList = new List<TodoItem>();
            foreach (var todoItemCreatedEvent in todoItemCreatedEvents)
            {
                var todoItem = await this.GetTodoAsync(todoItemCreatedEvent.Id);
                todoList.Add(todoItem);
            }

            return todoList;


            //          var todoItemList = new List<TodoItem>();
            // var createdTodoItemEvents = await this._eventStore.GetAll<TodoItemCreated>();
            // var discardedEvents = await this._eventStore.GetAll<TodoItemDiscarded>();
            // var finishedEvents = await this._eventStore.GetAll<TodoItemMarkedAsFinished>();
            // var resettedEvents = await this._eventStore.GetAll<TodoItemResetted>();
            // var titleUpdatedEvents = await this._eventStore.GetAll<TodoItemTitleUpdated>();
            // var orderUpdatedEvents = await this._eventStore.GetAll<TodoItemOrderUpdated>();
            // var restoredEvents = await this._eventStore.GetAll<TodoItemRestored>();
            //
            // foreach (var todoItemCreated in createdTodoItemEvents)
            // {
            // 	var eventStream = new List<IDomainEvent>();
            // 	eventStream.Add(todoItemCreated);
            // 	eventStream.AddRange(discardedEvents.Where(x => x.Id == todoItemCreated.Id));
            // 	eventStream.AddRange(finishedEvents.Where(x => x.Id == todoItemCreated.Id));
            // 	eventStream.AddRange(resettedEvents.Where(x => x.Id == todoItemCreated.Id));
            // 	eventStream.AddRange(titleUpdatedEvents.Where(x => x.Id == todoItemCreated.Id));
            // 	eventStream.AddRange(orderUpdatedEvents.Where(x => x.Id == todoItemCreated.Id));
            // 	eventStream.AddRange(restoredEvents.Where(x => x.Id == todoItemCreated.Id));
            // 	if (goBackMinutes != null)
            // 	{
            // 		eventStream = eventStream.Where(x => x.CreatedOn.IsOlderThan(TimeSpan.FromSeconds(goBackMinutes.Value))).ToList();
            // 	}
            // 	var orderedEventStream = eventStream.OrderBy(x => x.CreatedOn);
            // 	
            //
            // 	var todoItem = new TodoItem(orderedEventStream);
            //
            // 	if (todoItem.IsDiscarded)
            // 	{
            // 		continue;
            // 	}
            // 	if (todoItem.Id == null)
            // 	{
            // 		continue;
            // 	}
            // 	todoItemList.Add(todoItem);
            // }
            //
            // return todoItemList;
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