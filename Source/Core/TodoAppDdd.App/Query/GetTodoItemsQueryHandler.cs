﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TodoAppDdd.App.Common;
using TodoAppDdd.App.Contracts.Query;
using TodoAppDdd.App.Contracts.Query.Handler;
using TodoAppDdd.Persistence;

namespace TodoAppDdd.App.Query
{
	public class GetTodoItemsQueryHandler : IGetTodoItemsQueryHandler
	{
		private readonly ITodoRepository _todoRepository;
		private readonly ITodoItemMapper _mapper;

		public GetTodoItemsQueryHandler(ITodoRepository todoRepository, ITodoItemMapper mapper)
		{
			this._todoRepository = todoRepository;
			this._mapper = mapper;
		}

		public async Task<IEnumerable<TodoItemDto>> Handle(GetTodoItemsQuery query)
		{
			var todoItems = await this._todoRepository.GetAllTodos(query.GoBackSeconds);

			var todoItemsDtoList = new List<TodoItemDto>();
			foreach (var todoItem in todoItems)
			{
				if (todoItem.IsDiscarded)
				{
					continue;
				}

				var todoItemDto = this._mapper.Map(todoItem);
				todoItemsDtoList.Add(todoItemDto);
			}


			return todoItemsDtoList;
		}
	}
}
