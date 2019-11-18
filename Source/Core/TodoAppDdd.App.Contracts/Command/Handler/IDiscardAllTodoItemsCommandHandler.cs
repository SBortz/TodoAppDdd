using System;
using System.Collections.Generic;
using System.Text;

namespace TodoAppDdd.App.Contracts.Command.Handler
{
	public interface IDiscardAllTodoItemsCommandHandler
	{
		void Handle(DiscardAllTodoItemsCommand itemCommand);
	}
}
