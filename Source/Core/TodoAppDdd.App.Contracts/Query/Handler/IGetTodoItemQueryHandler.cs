using System.Collections.Generic;

namespace TodoAppDdd.App.Contracts.Query.Handler
{
	public interface IGetTodoItemQueryHandler
	{
		TodoItemDto Handle(GetTodoItemQuery query);
	}
}