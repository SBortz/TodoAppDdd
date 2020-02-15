using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TodoAppDdd.App.Contracts.Command;
using TodoAppDdd.App.Contracts.Command.Handler;
using TodoAppDdd.Persistence;

namespace TodoAppDdd.App.Command
{
	public class FinishTodoItemCommandHandler : IFinishTodoItemCommandHandler
	{
		private readonly ITodoRepository _todoRepository;
		private readonly ITodoItemReadModelRepository _readModelRepository;

		public FinishTodoItemCommandHandler(ITodoRepository todoRepository, ITodoItemReadModelRepository readModelRepository)
		{
			this._todoRepository = todoRepository;
			this._readModelRepository = readModelRepository;
		}

		public async Task Handle(FinishTodoItemCommand cmd)
		{
			var todoItem = await this._todoRepository.GetTodo(cmd.Id);

			todoItem.Finish();

			await this._todoRepository.SaveState(todoItem);
			this._readModelRepository.InsertOrUpdateFromTodoItem(todoItem);
		}
	}
}
