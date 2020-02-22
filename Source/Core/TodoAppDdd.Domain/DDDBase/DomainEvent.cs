using System;
using Newtonsoft.Json;

namespace TodoAppDdd.Domain.DDDBase
{
	public class DomainEvent : IDomainEvent
	{
		[JsonProperty(Order = -4)]
		public string Id { get; set; }

		[JsonProperty(Order = -3)]
		public string EventType { get; set; }

		[JsonProperty(Order = -2)]
		public DateTime CreatedOn { get; set; }
	}
}