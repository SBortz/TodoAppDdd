using TodoAppDdd.App.Contracts.Command;
using TodoAppDdd.App.Contracts.Command.Handler;
using TodoAppDdd.Persistence;

namespace TodoAppDdd.App.Command
{
	public class ResetTodoItemCommandHandler : IResetTodoItemCommandHandler
	{
		private ITodoRepository _todoRepository;

		public ResetTodoItemCommandHandler(ITodoRepository todoRepository)
		{
			this._todoRepository = todoRepository;
		}

		public void Handle(ResetTodoItemCommand cmd)
		{
			var todoItem = this._todoRepository.GetTodo(cmd.Id);

			todoItem.Reset();

			this._todoRepository.SaveState(todoItem);
		}
	}
}