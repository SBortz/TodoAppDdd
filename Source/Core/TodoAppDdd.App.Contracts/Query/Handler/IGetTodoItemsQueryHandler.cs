using System.Collections.Generic;
using System.Threading.Tasks;

namespace TodoAppDdd.App.Contracts.Query.Handler
{
	public interface IGetTodoItemsQueryHandler
	{
		Task<IEnumerable<TodoItemDto>> Handle(GetTodoItemsQuery query);
	}
}