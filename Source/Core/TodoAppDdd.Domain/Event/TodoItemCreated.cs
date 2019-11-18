using TodoAppDdd.Domain.Aggregate;
using TodoAppDdd.Domain.DDDBase;

namespace TodoAppDdd.Domain.Event
{
	public class TodoItemCreated : DomainEvent
	{
		public string Title { get; set; }
		public int? Order { get; set; }
	}
}
