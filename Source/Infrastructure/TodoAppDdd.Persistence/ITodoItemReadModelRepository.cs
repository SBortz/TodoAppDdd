using System.Collections.Generic;
using TodoAppDdd.Domain.Aggregate;
using TodoAppDdd.Domain.ReadModel;

namespace TodoAppDdd.Persistence
{
	public interface ITodoItemReadModelRepository
	{
		void InsertOrUpdateFromTodoItem(TodoItem todoItem);
		TodoItemReadModel Find(string todoItemId);
		IEnumerable<TodoItemReadModel> FindAll();
	}
}