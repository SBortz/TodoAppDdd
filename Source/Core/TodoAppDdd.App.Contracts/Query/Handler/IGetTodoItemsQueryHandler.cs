using System.Collections.Generic;

namespace TodoAppDdd.App.Contracts.Query.Handler
{
	public interface IGetTodoItemsQueryHandler
	{
		IEnumerable<TodoItemDto> Handle(GetTodoItemsQuery query);
	}
}