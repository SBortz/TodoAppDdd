using System.Collections.Generic;
using System.Threading.Tasks;
using TodoAppDdd.Domain.Aggregate;

namespace TodoAppDdd.Persistence
{
	public interface ITodoRepository
	{
		Task<TodoItem> GetTodoAsync(string id);
		Task<IEnumerable<TodoItem>> GetAllTodos(int? goBackMinutes = null);
		Task SaveState(TodoItem todoItem);
		Task<IEnumerable<TodoItem>> GetLastDiscardedTodos();
	}
}