using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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

		public GetTodoItemQueryHandler(ITodoItemMapper mapper, ITodoRepository todoRepository)
		{
			this._todoRepository = todoRepository;
			this._mapper = mapper;
		}

		public async Task<TodoItemDto> Handle(GetTodoItemQuery query)
		{
			var todoItem = await this._todoRepository.GetTodoAsync(query.Id);

			if (todoItem == null || todoItem.IsDiscarded)
			{
				return null;
			}

			var todoItemDto = this._mapper.Map(todoItem);
			return todoItemDto;
		}
	}
}
