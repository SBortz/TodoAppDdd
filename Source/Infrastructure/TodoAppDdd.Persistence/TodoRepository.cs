using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoAppDdd.Domain.Aggregate;
using TodoAppDdd.Domain.DDDBase;
using TodoAppDdd.Domain.Event;
using TodoAppDdd.Persistence.EventStore;

namespace TodoAppDdd.Persistence
{
	public class TodoRepository : ITodoRepository
	{
		private readonly IEventStore _eventStore;

		public TodoRepository(IEventStore eventStore)
		{
			this._eventStore = eventStore;
		}

		public async Task<TodoItem> GetTodo(string id)
		{
			var allNecessaryEvents = await this._eventStore.Get<TodoItemCreated>(id);
			var discardedEvents = await this._eventStore.Get<TodoItemDiscarded>(id);
			var finishedEvents = await this._eventStore.Get<TodoItemMarkedAsFinished>(id);
			var resettedEvents = await this._eventStore.Get<TodoItemResetted>(id);
			var titleUpdatedEvents = await this._eventStore.Get<TodoItemTitleUpdated>(id);
			var orderUpdatedEvents = await this._eventStore.Get<TodoItemOrderUpdated>(id);
			var restoredEvents = await this._eventStore.Get<TodoItemRestored>(id);

			var eventStream = new List<IDomainEvent>();
			eventStream.AddRange(allNecessaryEvents);
			eventStream.AddRange(discardedEvents);
			eventStream.AddRange(finishedEvents);
			eventStream.AddRange(resettedEvents);
			eventStream.AddRange(titleUpdatedEvents);
			eventStream.AddRange(orderUpdatedEvents);
			eventStream.AddRange(restoredEvents);

			var todoItem = new TodoItem(eventStream.OrderBy(x => x.CreatedOn));

			if (todoItem.Id == null)
			{
				return null;
			}

			return todoItem;
		}

		public async Task<IEnumerable<TodoItem>> GetAllTodos(int? goBackMinutes = null)
		{
			var todoItemList = new List<TodoItem>();
			var createdTodoItemEvents = await this._eventStore.GetAll<TodoItemCreated>();
			var discardedEvents = await this._eventStore.GetAll<TodoItemDiscarded>();
			var finishedEvents = await this._eventStore.GetAll<TodoItemMarkedAsFinished>();
			var resettedEvents = await this._eventStore.GetAll<TodoItemResetted>();
			var titleUpdatedEvents = await this._eventStore.GetAll<TodoItemTitleUpdated>();
			var orderUpdatedEvents = await this._eventStore.GetAll<TodoItemOrderUpdated>();
			var restoredEvents = await this._eventStore.GetAll<TodoItemRestored>();
			
			foreach (var todoItemCreated in createdTodoItemEvents)
			{
				var eventStream = new List<IDomainEvent>();
				eventStream.Add(todoItemCreated);
				eventStream.AddRange(discardedEvents.Where(x => x.Id == todoItemCreated.Id));
				eventStream.AddRange(finishedEvents.Where(x => x.Id == todoItemCreated.Id));
				eventStream.AddRange(resettedEvents.Where(x => x.Id == todoItemCreated.Id));
				eventStream.AddRange(titleUpdatedEvents.Where(x => x.Id == todoItemCreated.Id));
				eventStream.AddRange(orderUpdatedEvents.Where(x => x.Id == todoItemCreated.Id));
				eventStream.AddRange(restoredEvents.Where(x => x.Id == todoItemCreated.Id));
				if (goBackMinutes != null)
				{
					eventStream = eventStream.Where(x => x.CreatedOn.IsOlderThan(TimeSpan.FromSeconds(goBackMinutes.Value))).ToList();
				}
				var orderedEventStream = eventStream.OrderBy(x => x.CreatedOn);
				

				var todoItem = new TodoItem(orderedEventStream);

				if (todoItem.IsDiscarded)
				{
					continue;
				}
				if (todoItem.Id == null)
				{
					continue;
				}
				todoItemList.Add(todoItem);
			}
			
			return todoItemList;
		}

		public async Task<IEnumerable<TodoItem>> GetLastDiscardedTodos()
		{
			var discardedEvents = await this._eventStore.GetAll<TodoItemDiscarded>();
			var lastEvents = discardedEvents
				.OrderByDescending(x => x.CreatedOn);
				

			var todos = new List<TodoItem>();
			foreach (var todoItemDiscarded in lastEvents)
			{
				var todo = await this.GetTodo(todoItemDiscarded.Id);

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
	}
}