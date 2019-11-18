namespace TodoAppDdd.App.Contracts.Command.Handler
{
	public interface IFinishTodoItemCommandHandler
	{
		void Handle(FinishTodoItemCommand cmd);
	}
}