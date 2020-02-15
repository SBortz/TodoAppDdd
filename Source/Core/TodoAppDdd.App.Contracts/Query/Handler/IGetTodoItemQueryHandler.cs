using System.Collections.Generic;
using System.Threading.Tasks;

namespace TodoAppDdd.App.Contracts.Query.Handler
{
	public interface IGetTodoItemQueryHandler
	{
		Task<TodoItemDto> Handle(GetTodoItemQuery query);
	}
}