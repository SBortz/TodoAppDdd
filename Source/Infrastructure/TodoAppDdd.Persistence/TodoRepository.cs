using System;
using System.Collections.Generic;
using System.Linq;
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

		public TodoItem GetTodo(string id)
		{
			var allNecessaryEvents = this._eventStore.Get<TodoItemCreated>(id);
			var discardedEvents = this._eventStore.Get<TodoItemDiscarded>(id);
			var finishedEvents = this._eventStore.Get<TodoItemMarkedAsFinished>(id);
			var resettedEvents = this._eventStore.Get<TodoItemResetted>(id);
			var titleUpdatedEvents = this._eventStore.Get<TodoItemTitleUpdated>(id);
			var orderUpdatedEvents = this._eventStore.Get<TodoItemOrderUpdated>(id);
			var restoredEvents = this._eventStore.Get<TodoItemRestored>(id);

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

		public IEnumerable<TodoItem> GetAllTodos(int? goBackMinutes = null)
		{
			var todoItemList = new List<TodoItem>();
			var createdTodoItemEvents = this._eventStore.GetAll<TodoItemCreated>();
			var discardedEvents = this._eventStore.GetAll<TodoItemDiscarded>();
			var finishedEvents = this._eventStore.GetAll<TodoItemMarkedAsFinished>();
			var resettedEvents = this._eventStore.GetAll<TodoItemResetted>();
			var titleUpdatedEvents = this._eventStore.GetAll<TodoItemTitleUpdated>();
			var orderUpdatedEvents = this._eventStore.GetAll<TodoItemOrderUpdated>();
			var restoredEvents = this._eventStore.GetAll<TodoItemRestored>();
			
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
					eventStream = eventStream.Where(x => x.CreatedOn.IsOlderThan(TimeSpan.FromMinutes(goBackMinutes.Value))).ToList();
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

		public IEnumerable<TodoItem> GetLast5DiscardedTodos()
		{
			var discardedEvents = this._eventStore.GetAll<TodoItemDiscarded>();
			var lastEvents = discardedEvents
				.OrderByDescending(x => x.CreatedOn);
				

			var todos = new List<TodoItem>();
			foreach (var todoItemDiscarded in lastEvents)
			{
				var todo = this.GetTodo(todoItemDiscarded.Id);

				if (todo.IsDiscarded)
				{
					todos.Add(todo);
				}
				
				if (todos.Count >= 5)
				{
					return todos;
				}
			}

			return todos;
		}

		public void SaveState(TodoItem todoItem)
		{
			this._eventStore.AppendEvents(todoItem.DomainEvents);
		}
	}
}