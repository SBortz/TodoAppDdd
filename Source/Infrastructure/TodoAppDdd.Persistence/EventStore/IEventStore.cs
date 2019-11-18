using System;
using System.Collections.Generic;
using TodoAppDdd.Domain.DDDBase;

namespace TodoAppDdd.Persistence.EventStore
{
	public interface IEventStore
	{
		void AppendEvents(IEnumerable<IDomainEvent> domainEvents);

		IEnumerable<EventType> Get<EventType>(string id)
			where EventType : class, IDomainEvent;

		IEnumerable<EventType> GetAll<EventType>()
			where EventType : class, IDomainEvent;

		IEnumerable<String> OutputRawEvents();
		void DropAllEvents();
	}
}