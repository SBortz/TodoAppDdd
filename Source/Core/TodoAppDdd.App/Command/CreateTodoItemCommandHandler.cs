using System;
using System.Collections.Generic;
using System.Text;
using TodoAppDdd.App.Common;
using TodoAppDdd.App.Contracts.Command;
using TodoAppDdd.App.Contracts.Query;
using TodoAppDdd.Domain.Aggregate;
using TodoAppDdd.Persistence;

namespace TodoAppDdd.App.Command
{
	public class CreateTodoItemCommandHandler : ICreateTodoItemCommandHandler
	{
		private readonly ITodoRepository _todoRepository;
		private readonly ITodoItemMapper mapper;

		public CreateTodoItemCommandHandler(ITodoRepository todoRepository, ITodoItemMapper mapper)
		{
			this._todoRepository = todoRepository;
			this.mapper = mapper;
		}

		public TodoItemDto Handle(CreateTodoItemCommand itemCommand)
		{
			var todoItem = new TodoItem(itemCommand.Title, itemCommand.Order);

			this._todoRepository.SaveState(todoItem);

			return this.mapper.Map(todoItem);
		}
	}
}
