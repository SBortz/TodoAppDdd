using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TodoAppDdd.App.Contracts.Command;
using TodoAppDdd.App.Contracts.Command.Handler;
using TodoAppDdd.Persistence;

namespace TodoAppDdd.App.Command
{
	public class RestoreDiscardedTodoItemsCommandHandler : IRestoreDiscardedTodoItemsCommandHandler
	{
		private readonly ITodoRepository _todoRepository;
		private readonly ITodoItemReadModelRepository _readModelRepository;

		public RestoreDiscardedTodoItemsCommandHandler(ITodoRepository todoRepository, ITodoItemReadModelRepository readModelRepository)
		{
			this._todoRepository = todoRepository;
			this._readModelRepository = readModelRepository;
		}

		public async Task Handle(RestoreDiscardedTodoItemsCommand command)
		{
			var last5DiscardedTodos = await this._todoRepository.GetLastDiscardedTodos();
			foreach (var todoItem in last5DiscardedTodos)
			{
				todoItem.Restore();
				await this._todoRepository.SaveState(todoItem);
				this._readModelRepository.InsertOrUpdateFromTodoItem(todoItem);
			}
		}
	}
}
