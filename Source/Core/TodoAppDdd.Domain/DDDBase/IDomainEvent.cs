using System;
using Newtonsoft.Json;

namespace TodoAppDdd.Domain.DDDBase
{
	public interface IDomainEvent
	{
		string Id { get; set; }

		string EventType { get; set; }

		DateTime CreatedOn { get; set; }
	}
}