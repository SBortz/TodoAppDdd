using System;
using TodoAppDdd.Domain.DDDBase;

namespace TodoAppDdd.Persistence.EventStore
{
	public class StoredEvent : IDomainEvent
	{
		public string Id { get; set; }
		public string EventType { get; set; }
		public DateTime CreatedOn { get; set; }
	}
}