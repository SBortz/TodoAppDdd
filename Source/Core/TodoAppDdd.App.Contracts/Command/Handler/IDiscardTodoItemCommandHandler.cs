using System.Threading.Tasks;

namespace TodoAppDdd.App.Contracts.Command.Handler
{
	public interface IDiscardTodoItemCommandHandler
	{
		Task Handle(DiscardTodoItemCommand cmd);
	}
}