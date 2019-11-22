using System;
using System.Collections.Generic;
using System.Text;
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

		public void Handle(RestoreDiscardedTodoItemsCommand command)
		{
			var last5DiscardedTodos = this._todoRepository.GetLast5DiscardedTodos();
			foreach (var todoItem in last5DiscardedTodos)
			{
				todoItem.Restore();
				this._todoRepository.SaveState(todoItem);
				this._readModelRepository.InsertOrUpdateFromTodoItem(todoItem);
			}
		}
	}
}
