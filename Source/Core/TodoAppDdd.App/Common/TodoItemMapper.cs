using TodoAppDdd.App.Contracts.Query;
using TodoAppDdd.Domain.Aggregate;

namespace TodoAppDdd.App.Common
{
	public class TodoItemMapper : ITodoItemMapper
	{
		public TodoItemDto Map(TodoItem todoItem)
		{
			return new TodoItemDto()
			{
				Id = todoItem.Id,
				Title = todoItem.Title,
				Completed = todoItem.IsFinished,
				Order = todoItem.Order,
				CreatedOn = todoItem.CreatedOn,
				LastUpdate = todoItem.LastUpdate
			};
		}
	}
}