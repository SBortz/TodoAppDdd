using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TodoAppDdd.Domain.DDDBase;

namespace TodoAppDdd.Persistence.EventStore
{
	public class InMemoryEventStore : IEventStore
	{
		private readonly List<IDomainEvent> _events = new List<IDomainEvent>();

		public async Task AppendEvents(IEnumerable<IDomainEvent> domainEvents)
		{
			foreach (var domainEvent in domainEvents)
			{
				this._events.Add(domainEvent);
			}
		}

		public async Task<IEnumerable<TEventType>> Get<TEventType>(string id) 
			where TEventType : class, IDomainEvent
		{
			var selectedEvents = _events
				.Where(x => x.Id == id);
			List<TEventType> domainEvents = selectedEvents
				.Where(x => x.EventType == typeof(TEventType).Name)
				.Select(storedEvent => storedEvent as TEventType)
				.ToList();

			return domainEvents;
		}

        public async Task<IEnumerable<TEventType>> GetAll<TEventType>() 
			where TEventType : class, IDomainEvent
		{
			var domainEvents = this._events
				.Where(x => x.EventType == typeof(TEventType).Name)
				.Select(storedEvent => storedEvent as TEventType)
				.ToList();

			return domainEvents;
		}

		#region These functions exist, because they make it easier to demonstrate EventSourcing.
		public IEnumerable<string> OutputRawEvents()
		{
			return this._events
				.OrderBy(x => x.CreatedOn)
				.Select(JsonConvert.SerializeObject);
		}

		public void DropAllEvents()
		{
			this._events.Clear();
		}
		#endregion
	}
}