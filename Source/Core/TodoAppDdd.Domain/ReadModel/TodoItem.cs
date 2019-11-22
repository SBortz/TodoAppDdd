using System;
using System.Collections.Generic;
using System.Text;

namespace TodoAppDdd.Domain.ReadModel
{
	public class TodoItemReadModel
	{
		public string Id { get; set; }
		public string Title { get; set; }
		public int? Order { get; set; }
		public bool IsFinished { get; set; }
		public DateTime CreatedOn { get; set; }
		public DateTime LastUpdate { get; set; }
		public bool IsDiscarded { get; set; }
	}
}
