using System;
using System.Collections.Generic;
using System.Text;

namespace TodoAppDdd.App.Contracts.Command
{
	public class UpdateAllFieldsOfTodoItemCommand
	{
		public string Id { get; set; }
		public string Title { get; set; }
		public int? Order { get; set; }
		public bool? Completed { get; set; }
	}
}
