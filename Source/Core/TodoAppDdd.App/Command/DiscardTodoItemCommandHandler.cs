using System;
using System.Collections.Generic;
using System.Text;
using TodoAppDdd.App.Contracts.Command;
using TodoAppDdd.App.Contracts.Command.Handler;
using TodoAppDdd.Persistence;

namespace TodoAppDdd.App.Command
{
	public class DiscardTodoItemCommandHandler : IDiscardTodoItemCommandHandler
	{
		private ITodoRepository todoRepository;

		public DiscardTodoItemCommandHandler(ITodoRepository todoRepository)
		{
			this.todoRepository = todoRepository;
		}

		public void Handle(DiscardTodoItemCommand cmd)
		{
			var todoItem = this.todoRepository.GetTodo(cmd.Id);

			todoItem.Discard();

			this.todoRepository.SaveState(todoItem);
		}
	}
}
