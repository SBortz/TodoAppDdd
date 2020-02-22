using System.Threading.Tasks;
using TodoAppDdd.App.Contracts.Command;
using TodoAppDdd.App.Contracts.Command.Handler;
using TodoAppDdd.Persistence;

namespace TodoAppDdd.App.Command
{
	public class ResetTodoItemCommandHandler : IResetTodoItemCommandHandler
	{
		private readonly ITodoRepository _todoRepository;
		private readonly ITodoItemReadModelRepository _readModelRepository;

		public ResetTodoItemCommandHandler(ITodoRepository todoRepository, ITodoItemReadModelRepository readModelRepository)
		{
			this._todoRepository = todoRepository;
			this._readModelRepository = readModelRepository;
		}

		public async Task Handle(ResetTodoItemCommand cmd)
		{
			var todoItem = await this._todoRepository.GetTodoAsync(cmd.Id);

			todoItem.Reset();

			await this._todoRepository.SaveState(todoItem);
			this._readModelRepository.InsertOrUpdateFromTodoItem(todoItem);
		}
	}
}