using System.Collections.Generic;
using TodoAppDdd.Domain.Aggregate;
using TodoAppDdd.Domain.ReadModel;

namespace TodoAppDdd.Persistence
{
	public class FakeTodoItemReadModelRepository : ITodoItemReadModelRepository
	{
		public void InsertOrUpdateFromTodoItem(TodoItem todoItem)
		{
		}

		public TodoItemReadModel Find(string todoItemId)
		{
			return null;
		}

		public IEnumerable<TodoItemReadModel> FindAll()
		{
			return null;
		}
	}
}