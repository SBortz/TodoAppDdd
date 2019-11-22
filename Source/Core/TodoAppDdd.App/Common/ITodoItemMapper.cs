using TodoAppDdd.App.Contracts.Query;
using TodoAppDdd.Domain.Aggregate;
using TodoAppDdd.Domain.ReadModel;

namespace TodoAppDdd.App.Common
{
	public interface ITodoItemMapper
	{
		TodoItemDto Map(TodoItem todoItem);
		TodoItemDto Map(TodoItemReadModel todoItem);
	}
}