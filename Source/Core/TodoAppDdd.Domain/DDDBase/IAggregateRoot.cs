using TodoAppDdd.Domain.Aggregate;

namespace TodoAppDdd.Domain.DDDBase
{
	public interface IAggregateRoot
	{
		void RaiseEvent(IDomainEvent domainEvent);
	}
}