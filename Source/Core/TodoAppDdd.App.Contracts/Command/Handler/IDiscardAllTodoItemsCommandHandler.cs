using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TodoAppDdd.App.Contracts.Command.Handler
{
	public interface IDiscardAllTodoItemsCommandHandler
	{
		Task Handle(DiscardAllTodoItemsCommand itemCommand);
	}
}
