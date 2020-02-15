using System.Threading.Tasks;

namespace TodoAppDdd.App.Contracts.Command.Handler
{
	public interface IResetTodoItemCommandHandler
	{
		Task Handle(ResetTodoItemCommand cmd);
	}
}