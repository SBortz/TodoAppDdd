using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TodoAppDdd.App.Contracts.Query;

namespace TodoAppDdd.App.Contracts.Command
{
	public interface ICreateTodoItemCommandHandler
	{
		Task<TodoItemDto> Handle(CreateTodoItemCommand itemCommand);
	}
}
