using TodoAppDdd.App.Contracts.Query;
using TodoAppDdd.Domain.Aggregate;

namespace TodoAppDdd.App.Common
{
	public interface ITodoItemMapper
	{
		TodoItemDto Map(TodoItem todoItem);
	}
}