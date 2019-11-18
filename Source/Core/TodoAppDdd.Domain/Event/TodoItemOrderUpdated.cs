using TodoAppDdd.Domain.DDDBase;

namespace TodoAppDdd.Domain.Event
{
	public class TodoItemOrderUpdated : DomainEvent
	{
		public int Order { get; set; }
	}
}