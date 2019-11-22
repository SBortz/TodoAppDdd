using System;
using System.Collections.Generic;
using System.Text;
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

		public void Handle(FinishTodoItemCommand cmd)
		{
			var todoItem = this._todoRepository.GetTodo(cmd.Id);

			todoItem.Finish();

			this._todoRepository.SaveState(todoItem);
			this._readModelRepository.InsertOrUpdateFromTodoItem(todoItem);
		}
	}
}
