using NUnit.Framework;
using System.Linq;
using TodoAppDdd.Domain.Aggregate;
using TodoAppDdd.Domain.Event;

namespace TodoAppDdd.App.Tests
{
	public class TodoItemTests
	{
		private TodoItem todoItem;
		private string initialTodoText = "Was machen";

		[SetUp]
		public void Setup()
		{
			this.todoItem = new TodoItem(initialTodoText);
		}

		[Test]
		public void InstantiateCorrectly()
		{
			Assert.That(todoItem.Title == initialTodoText);
			Assert.That(todoItem.IsDiscarded == false);
			Assert.That(todoItem.IsFinished == false);
		}

		[Test]
		public void InstantiateEmitsCreatedEvent()
		{
			Assert.That(todoItem.DomainEvents.Count() == 1);

			var firstEvent = todoItem.DomainEvents.LastOrDefault();
			Assert.That(firstEvent.GetType() == typeof(TodoItemCreated));
		}

		[Test]
		public void DiscardTodoItemDiscards()
		{
			todoItem.Discard();

			Assert.That(todoItem.IsDiscarded);
		}

		[Test]
		public void DiscardEmitsDiscardedEvent()
		{
			todoItem.Discard();

			var lastEvent = todoItem.DomainEvents.LastOrDefault();
			Assert.That(lastEvent.GetType() == typeof(TodoItemDiscarded));
		}

		[Test]
		public void FinishEmitsFinishedEvent()
		{
			todoItem.Finish();

			var lastEvent = todoItem.DomainEvents.LastOrDefault();
			Assert.That(lastEvent.GetType() == typeof(TodoItemMarkedAsFinished));
		}

	}
}