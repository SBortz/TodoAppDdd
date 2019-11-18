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
		private readonly ITodoRepository todoRepository;

		public RestoreDiscardedTodoItemsCommandHandler(ITodoRepository todoRepository)
		{
			this.todoRepository = todoRepository;
		}

		public void Handle(RestoreDiscardedTodoItemsCommand command)
		{
			var last5DiscardedTodos = this.todoRepository.GetLast5DiscardedTodos();
			foreach (var todoItem in last5DiscardedTodos)
			{
				todoItem.Restore();
				this.todoRepository.SaveState(todoItem);
			}
		}
	}
}
