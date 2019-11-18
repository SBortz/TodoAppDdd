using System;
using System.Collections.Generic;
using System.Text;
using TodoAppDdd.App.Common;
using TodoAppDdd.App.Contracts.Query;
using TodoAppDdd.App.Contracts.Query.Handler;
using TodoAppDdd.Persistence;

namespace TodoAppDdd.App.Query
{
	public class GetTodoItemQueryHandler : IGetTodoItemQueryHandler
	{
		private readonly ITodoRepository _todoRepository;
		private readonly ITodoItemMapper _mapper;

		public GetTodoItemQueryHandler(ITodoRepository todoRepository, ITodoItemMapper mapper)
		{
			this._todoRepository = todoRepository;
			this._mapper = mapper;
		}

		public TodoItemDto Handle(GetTodoItemQuery query)
		{
			var todoItem = this._todoRepository.GetTodo(query.Id);

			if (todoItem == null || todoItem.IsDiscarded)
			{
				return null;
			}

			var todoItemDto = this._mapper.Map(todoItem);
			return todoItemDto;
		}
	}
}
