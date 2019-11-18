using System;

namespace TodoAppDdd.App.Contracts.Query
{
	public class TodoItemDto
	{
		public string Id { get; set; }
		public string Title { get; set; }
		public bool? Completed { get; set; }
		public int? Order { get; set; }

		public string Url
		{
			get { return "https://todoappdddapi.azurewebsites.net/api/Todo/" + Id; }
		}

		public DateTime CreatedOn { get; set; }

		public DateTime LastUpdate { get; set; }
	}
}