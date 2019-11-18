using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Net.Http.Headers;
using System.Text;
using TodoAppDdd.Domain.Aggregate.Exceptions;
using TodoAppDdd.Domain.DDDBase;
using TodoAppDdd.Domain.Event;

namespace TodoAppDdd.Domain.Aggregate
{
	public class TodoItem : AggregateRoot
	{
		public string Id { get; private set; }
		public string Title { get; private set; }
		public bool IsDiscarded { get; private set; }
		public bool IsFinished { get; private set; }
		public int? Order { get; private set; }
		public DateTime CreatedOn { get; private set; }
		public DateTime LastUpdate { get; private set; }

		public TodoItem(IEnumerable<IDomainEvent> domainEvents)
			: base(domainEvents)
		{
		}

		protected override void When(IDomainEvent e)
		{
			if(e is TodoItemCreated) When(e as TodoItemCreated);
			else if(e is TodoItemMarkedAsFinished) When(e as TodoItemMarkedAsFinished);
			else if(e is TodoItemDiscarded) When(e as TodoItemDiscarded);
			else if(e is TodoItemResetted) When(e as TodoItemResetted);
			else if(e is TodoItemTitleUpdated) When(e as TodoItemTitleUpdated);
			else if(e is TodoItemOrderUpdated) When(e as TodoItemOrderUpdated);
			else if(e is TodoItemRestored) When(e as TodoItemRestored);
		}

		public TodoItem(string text, int? order = null)
		{
			this.RaiseEvent(new TodoItemCreated()
			{
				Id = Guid.NewGuid().ToString(),
				Title = text,
				Order = order
			});
		}

		private void When(TodoItemCreated e)
		{
			this.Id = e.Id;
			this.Title = e.Title;
			this.Order = e.Order;
			this.CreatedOn = e.CreatedOn;
			this.LastUpdate = e.CreatedOn;
		}

		public void Finish()
		{
			if (this.IsFinished || this.IsDiscarded)
			{
				throw new TodoItemAlreadyDiscardedException();
			}

			this.RaiseEvent(new TodoItemMarkedAsFinished()
			{
				Id = this.Id
			});
		}

		private void When(TodoItemMarkedAsFinished e)
		{
			this.IsFinished = true;
			this.LastUpdate = e.CreatedOn;
		}

		public void Discard()
		{
			if (this.IsDiscarded)
			{
				throw new TodoItemAlreadyDiscardedException();
			}

			this.RaiseEvent(new TodoItemDiscarded()
			{
				Id = this.Id
			});
		}

		private void When(TodoItemDiscarded e)
		{
			this.IsDiscarded = true;
			this.LastUpdate = e.CreatedOn;
		}

		public void Reset()
		{
			if (!this.IsFinished)
			{
				throw new TodoItemNotResettableException();
			}

			this.RaiseEvent(
				new TodoItemResetted()
				{
					Id = this.Id
				});
		}

		private void When(TodoItemResetted e)
		{
			this.IsFinished = false;
			this.LastUpdate = e.CreatedOn;
		}

		public void UpdateTitle(string newTitle)
		{
			this.RaiseEvent(new TodoItemTitleUpdated()
			{
				Id = this.Id,
				Title = newTitle
			});
		}

		private void When(TodoItemTitleUpdated e)
		{
			this.Title = e.Title;
			this.LastUpdate = e.CreatedOn;
		}

		public void UpdateOrder(int order)
		{
			this.RaiseEvent(new TodoItemOrderUpdated()
			{
				Id = this.Id,
				Order = order
			});
		}

		private void When(TodoItemOrderUpdated e)
		{
			this.Order = e.Order;
			this.LastUpdate = e.CreatedOn;
		}

		public void Restore()
		{
			if (!this.IsDiscarded)
			{
				throw new TodoItemIsNotRestorableException();
			}

			this.RaiseEvent(new TodoItemRestored()
			{
				Id = Id
			});
		}

		private void When(TodoItemRestored e)
		{
			this.IsDiscarded = false;
			this.LastUpdate = e.CreatedOn;
			this.CreatedOn = e.CreatedOn;
		}
	}
}
