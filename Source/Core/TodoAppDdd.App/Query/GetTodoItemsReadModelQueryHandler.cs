using System;
using System.Collections.Generic;
using System.Text;
using TodoAppDdd.App.Common;
using TodoAppDdd.App.Contracts.Query;
using TodoAppDdd.App.Contracts.Query.Handler;
using TodoAppDdd.Persistence;

namespace TodoAppDdd.App.Query
{
	public class GetTodoItemsReadModelQueryHandler : IGetTodoItemsQueryHandler
	{
		private readonly ITodoRepository _todoRepository;
		private readonly ITodoItemReadModelRepository _todoReadModelRepository;
		private readonly ITodoItemMapper _mapper;

		public GetTodoItemsReadModelQueryHandler(ITodoRepository todoRepository, ITodoItemMapper mapper, ITodoItemReadModelRepository todoReadModelRepository)
		{
			this._todoRepository = todoRepository;
			this._mapper = mapper;
			this._todoReadModelRepository = todoReadModelRepository;
		}

		public IEnumerable<TodoItemDto> Handle(GetTodoItemsQuery query)
		{
			if (query.GoBackSeconds.HasValue && query.GoBackSeconds.Value > 0)
			{
				return this.Fallback(query);
			}

			var todoItems = this._todoReadModelRepository.FindAll();

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

		private IEnumerable<TodoItemDto> Fallback(GetTodoItemsQuery query)
		{
			var todoItems = this._todoRepository.GetAllTodos(query.GoBackSeconds);

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
