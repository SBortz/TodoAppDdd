using System;
using System.Collections.Generic;
using System.Text;
using TodoAppDdd.App.Contracts.Query;

namespace TodoAppDdd.App.Contracts.Command
{
	public interface ICreateTodoItemCommandHandler
	{
		TodoItemDto Handle(CreateTodoItemCommand itemCommand);
	}
}
