using System.Threading.Tasks;

namespace TodoAppDdd.App.Contracts.Command.Handler
{
	public interface IRestoreDiscardedTodoItemsCommandHandler
	{
		Task Handle(RestoreDiscardedTodoItemsCommand command);
	}
}