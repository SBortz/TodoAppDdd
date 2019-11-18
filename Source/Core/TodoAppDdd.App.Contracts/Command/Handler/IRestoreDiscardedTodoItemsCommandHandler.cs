namespace TodoAppDdd.App.Contracts.Command.Handler
{
	public interface IRestoreDiscardedTodoItemsCommandHandler
	{
		void Handle(RestoreDiscardedTodoItemsCommand command);
	}
}