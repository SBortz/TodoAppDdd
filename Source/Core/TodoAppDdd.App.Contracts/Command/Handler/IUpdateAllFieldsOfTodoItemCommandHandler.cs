using System;
using System.Collections.Generic;
using System.Text;
using TodoAppDdd.App.Contracts.Query;

namespace TodoAppDdd.App.Contracts.Command.Handler
{
	public interface IUpdateAllFieldsOfTodoItemCommandHandler
	{
		TodoItemDto Handle(UpdateAllFieldsOfTodoItemCommand command);
	}
}
