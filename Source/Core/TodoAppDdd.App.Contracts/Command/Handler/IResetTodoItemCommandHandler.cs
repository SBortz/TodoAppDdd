namespace TodoAppDdd.App.Contracts.Command.Handler
{
	public interface IResetTodoItemCommandHandler
	{
		void Handle(ResetTodoItemCommand cmd);
	}
}