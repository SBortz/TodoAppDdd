using TodoAppDdd.Domain.DDDBase;

namespace TodoAppDdd.Domain.Event
{
	public class TodoItemTitleUpdated : DomainEvent
	{
		public string Title { get; set; }
	}
}