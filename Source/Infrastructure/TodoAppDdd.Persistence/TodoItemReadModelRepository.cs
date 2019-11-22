using System.Collections.Generic;
using System.Linq;
using TodoAppDdd.Domain.Aggregate;
using TodoAppDdd.Domain.ReadModel;

namespace TodoAppDdd.Persistence
{
	public class TodoItemReadModelRepository : ITodoItemReadModelRepository
	{
		private readonly TodoAppDddContext _context;

		public TodoItemReadModelRepository(TodoAppDddContext context)
		{
			this._context = context;
		}

		public void InsertOrUpdateFromTodoItem(TodoItem todoItem)
		{
			TodoItemReadModel currentTodoItemReadModel = this._context.TodoItem.FirstOrDefault(x => x.Id == todoItem.Id);
			if (currentTodoItemReadModel == null)
			{
				currentTodoItemReadModel = new TodoItemReadModel()
				{
					Id = todoItem.Id
				};
				this._context.TodoItem.Add(currentTodoItemReadModel);
			}

			currentTodoItemReadModel.Title = todoItem.Title;
			currentTodoItemReadModel.IsFinished = todoItem.IsFinished;
			currentTodoItemReadModel.Order = todoItem.Order;
			currentTodoItemReadModel.CreatedOn = todoItem.CreatedOn;
			currentTodoItemReadModel.LastUpdate = todoItem.LastUpdate;

			this._context.SaveChanges();
		}


		public TodoItemReadModel Find(string todoItemId)
		{
			return this._context.TodoItem.Find(todoItemId);
		}

		public IEnumerable<TodoItemReadModel> FindAll()
		{
			return this._context.TodoItem;
		}
	}
}