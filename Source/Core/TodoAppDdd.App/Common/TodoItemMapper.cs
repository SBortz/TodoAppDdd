using TodoAppDdd.App.Contracts.Query;
using TodoAppDdd.Domain.Aggregate;
using TodoAppDdd.Domain.ReadModel;

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

		public TodoItemDto Map(TodoItemReadModel todoItem)
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