using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoAppDdd.Domain.DDDBase;

namespace TodoAppDdd.Persistence.EventStore
{
	public interface IEventStore
	{
		Task AppendEvents(IEnumerable<IDomainEvent> domainEvents);

		Task<IEnumerable<EventType>> Get<EventType>(string id)
			where EventType : class, IDomainEvent;

		Task<IEnumerable<EventType>> GetAll<EventType>()
			where EventType : class, IDomainEvent;

		IEnumerable<String> OutputRawEvents();
		void DropAllEvents();
	}
}