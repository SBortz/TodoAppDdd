using System.Threading.Tasks;

namespace TodoAppDdd.App.Contracts.Command.Handler
{
	public interface IFinishTodoItemCommandHandler
	{
		Task Handle(FinishTodoItemCommand cmd);
	}
}