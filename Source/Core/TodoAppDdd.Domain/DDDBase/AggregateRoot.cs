using System;
using System.Collections.Generic;
using TodoAppDdd.Domain.Aggregate;
using TodoAppDdd.Domain.Event;

namespace TodoAppDdd.Domain.DDDBase
{
	public abstract class AggregateRoot : IAggregateRoot
	{
		private ICollection<IDomainEvent> _events;

		public IEnumerable<IDomainEvent> DomainEvents => new List<IDomainEvent>(this._events);

		public AggregateRoot()
		{
		}

		public AggregateRoot(IEnumerable<IDomainEvent> eventStream)
		{
			foreach (var e in eventStream)
			{
				this.When(e);
			}
		}

		public void RaiseEvent(IDomainEvent domainEvent)
		{
			if (this._events == null)
			{
				this._events = new List<IDomainEvent>();
			}

			domainEvent.CreatedOn = DateTime.Now;
			domainEvent.EventType = domainEvent.GetType().Name;

			this._events.Add(domainEvent);
			this.When(domainEvent);
		}

		protected abstract void When(IDomainEvent e);
	}
}