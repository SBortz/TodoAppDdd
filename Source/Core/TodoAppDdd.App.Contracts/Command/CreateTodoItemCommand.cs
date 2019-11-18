using System;
using System.Collections.Generic;
using System.Text;

namespace TodoAppDdd.App.Contracts.Command
{
	public class CreateTodoItemCommand
	{
		public string Title { get; set; }
		public int? Order { get; set; }
	}
}
