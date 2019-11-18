namespace TodoAppDdd.App.Contracts.Command.Handler
{
	public interface IDiscardTodoItemCommandHandler
	{
		void Handle(DiscardTodoItemCommand cmd);
	}
}