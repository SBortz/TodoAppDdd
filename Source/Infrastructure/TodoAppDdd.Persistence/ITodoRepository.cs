using System.Collections.Generic;
using TodoAppDdd.Domain.Aggregate;

namespace TodoAppDdd.Persistence
{
	public interface ITodoRepository
	{
		TodoItem GetTodo(string id);
		IEnumerable<TodoItem> GetAllTodos(int? goBackMinutes = null);
		void SaveState(TodoItem todoItem);
		IEnumerable<TodoItem> GetLastDiscardedTodos();
	}
}