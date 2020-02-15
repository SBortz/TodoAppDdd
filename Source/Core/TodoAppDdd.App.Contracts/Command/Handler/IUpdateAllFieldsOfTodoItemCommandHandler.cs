using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TodoAppDdd.App.Contracts.Query;

namespace TodoAppDdd.App.Contracts.Command.Handler
{
	public interface IUpdateAllFieldsOfTodoItemCommandHandler
	{
		Task<TodoItemDto> Handle(UpdateAllFieldsOfTodoItemCommand command);
	}
}
