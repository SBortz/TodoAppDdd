using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TodoAppDdd.Domain.Aggregate;
using TodoAppDdd.Domain.DDDBase;
using TodoAppDdd.Domain.Event;

namespace TodoAppDdd.Domain.Tests.Aggregate
{
	public class TodoItemTests
	{
		private TodoItem _todoItem;
		private string initialTodoText = "Was machen";

		[SetUp]
		public void Setup()
		{
			this._todoItem = new TodoItem(this.initialTodoText);
		}

		[Test]
		public void InstantiateCorrectly()
		{
			Assert.That(this._todoItem.Title == this.initialTodoText);
			Assert.That(this._todoItem.IsDiscarded == false);
			Assert.That(this._todoItem.IsFinished == false);
		}

		[Test]
		public void InstantiateEmitsCreatedEvent()
		{
			Assert.That(this._todoItem.DomainEvents.Count() == 1);

			var firstEvent = this._todoItem.DomainEvents.LastOrDefault();
			Assert.That(firstEvent.GetType() == typeof(TodoItemCreated));
		}

		[Test]
		public void DiscardTodoItemDiscards()
		{
			this._todoItem.Discard();

			Assert.That(this._todoItem.IsDiscarded);
		}

		[Test]
		public void DiscardEmitsDiscardedEvent()
		{
			this._todoItem.Discard();

			var lastEvent = this._todoItem.DomainEvents.LastOrDefault();
			Assert.That(lastEvent.GetType() == typeof(TodoItemDiscarded));
		}

		[Test]
		public void FinishEmitsFinishedEvent()
		{
			this._todoItem.Finish();

			var lastEvent = this._todoItem.DomainEvents.LastOrDefault();
			Assert.That(lastEvent.GetType() == typeof(TodoItemMarkedAsFinished));
		}

		#region Replay

		[Test]
		public void TodoItemReplaysEvents()
		{
			List<IDomainEvent> domainEvents = new List<IDomainEvent>();
			var id = "1";
			var dateTime = new DateTime();
			domainEvents.Add(new TodoItemCreated(){ Id = id, CreatedOn = dateTime, EventType = "TodoItemCreated", Title = "Start"});

			var todoItem = new TodoItem(domainEvents);
			
			Assert.That(todoItem.Title == "Start");
			Assert.That(todoItem.Id == id);
		}

		[Test]
		public void TodoItemReplaysEvents2()
		{
			List<IDomainEvent> domainEvents = new List<IDomainEvent>();
			var id = "1";
			var dateTime = new DateTime();
			domainEvents.Add(new TodoItemCreated() { Id = id, CreatedOn = dateTime, EventType = "TodoItemCreated", Title = "Todo" });
			dateTime = dateTime.AddSeconds(1);
			domainEvents.Add(new TodoItemTitleUpdated() { Id = id, CreatedOn = dateTime, EventType = "TodoItemTitleUpdated", Title = "EditedTodo" });
			dateTime = dateTime.AddSeconds(1);
			domainEvents.Add(new TodoItemTitleUpdated() { Id = id, CreatedOn = dateTime, EventType = "TodoItemTitleUpdated", Title = "AgainEditedTodo" });

			var todoItem = new TodoItem(domainEvents);

			Assert.That(todoItem.Title == "AgainEditedTodo");
			Assert.That(todoItem.Id == id);
		}

		[Test]
		public void TodoItemReplayThrowsOnEventsWithWrongAggregateId()
		{
			List<IDomainEvent> domainEvents = new List<IDomainEvent>();
			var id = "1";
			var dateTime = new DateTime();
			domainEvents.Add(new TodoItemCreated() { Id = id, CreatedOn = dateTime, EventType = "TodoItemCreated", Title = "Start" });
			dateTime = dateTime.AddSeconds(1);
			domainEvents.Add(new TodoItemTitleUpdated() { Id = "SomeOtherId", CreatedOn = dateTime, EventType = "TodoItemTitleUpdated", Title = "NewStart" });

			Assert.Throws<WrongIdOnAggregateException>(() =>
			{
				var todoItem = new TodoItem(domainEvents);
			});
		}

		#endregion
	}
}