using System;
using System.Collections.Generic;
using System.Text;
using TodoAppDdd.App.Common;
using TodoAppDdd.App.Contracts.Query;
using TodoAppDdd.App.Contracts.Query.Handler;
using TodoAppDdd.Domain.ReadModel;
using TodoAppDdd.Persistence;

namespace TodoAppDdd.App.Query
{
	public class GetTodoItemReadModelQueryHandler : IGetTodoItemQueryHandler
	{
		private readonly ITodoRepository _todoRepository;
		private readonly ITodoItemReadModelRepository _readModelRepository;
		private readonly ITodoItemMapper _mapper;

		public GetTodoItemReadModelQueryHandler(ITodoRepository todoRepository, ITodoItemMapper mapper, ITodoItemReadModelRepository readModelRepository)
		{
			this._todoRepository = todoRepository;
			this._mapper = mapper;
			this._readModelRepository = readModelRepository;
		}

		public TodoItemDto Handle(GetTodoItemQuery query)
		{
			var todoItem = this._readModelRepository.Find(query.Id);

			if (todoItem == null)
			{
				return this.Fallback(query);
			}

			if (todoItem.IsDiscarded)
			{
				return null;
			}

			var todoItemDto = this._mapper.Map(todoItem);
			return todoItemDto;
		}

		private TodoItemDto Fallback(GetTodoItemQuery query)
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
