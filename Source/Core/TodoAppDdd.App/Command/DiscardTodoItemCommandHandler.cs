using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TodoAppDdd.App.Contracts.Command;
using TodoAppDdd.App.Contracts.Command.Handler;
using TodoAppDdd.Persistence;

namespace TodoAppDdd.App.Command
{
	public class DiscardTodoItemCommandHandler : IDiscardTodoItemCommandHandler
	{
		private readonly ITodoRepository _todoRepository;
		private readonly ITodoItemReadModelRepository _readModelRepository;

		public DiscardTodoItemCommandHandler(ITodoRepository todoRepository, ITodoItemReadModelRepository readModelRepository)
		{
			this._todoRepository = todoRepository;
			this._readModelRepository = readModelRepository;
		}

		public async Task Handle(DiscardTodoItemCommand cmd)
		{
			var todoItem = await this._todoRepository.GetTodoAsync(cmd.Id);

			todoItem.Discard();

			await this._todoRepository.SaveState(todoItem);
			this._readModelRepository.InsertOrUpdateFromTodoItem(todoItem);
		}
	}
}
