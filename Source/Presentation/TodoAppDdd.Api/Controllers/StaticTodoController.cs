using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TodoAppDdd.Api.Controllers
{
	public class TodoItemDto
	{
		public int Id { get; set; }
		public int? Order { get; set; }
		public string Title { get; set; }
		public bool? Completed { get; set; }

		public string Url
		{
			get { return "https://todoappddd.azurewebsites.net/api/Todo/" + Id; }
		}
	}

	[Route("api/[controller]")]
	[ApiController]
	public class StaticTodoController : ControllerBase
	{
		private static List<TodoItemDto> todoItems = new List<TodoItemDto>()
		{
			new TodoItemDto()
			{
				Id = 1,
				Order = 1,
				Title = "1",
				Completed = false
			},
			new TodoItemDto()
			{
				Id = 2,
				Order = 2,
				Title = "2",
				Completed = false
			},
		};

		// GET api/values
		[HttpGet]
		public ActionResult<IEnumerable<TodoItemDto>> Get()
		{
			return todoItems;
		}

		// GET api/values/5
		[HttpGet("{id}")]
		public ActionResult<TodoItemDto> Get(int id)
		{
			return todoItems.FirstOrDefault(x => x.Id == id);
		}

		// POST api/values
		[HttpPost]
		public ActionResult Post([FromBody] TodoItemDto value)
		{
			value.Completed = false;

			todoItems.Add(value);
			return this.Ok(value);
		}

		// PUT api/values/5
		[HttpPut("{id}")]
		public ActionResult Put(int id, [FromBody] string value)
		{
			return this.Ok();
		}

		// PUT api/values/5
		[HttpPatch("{id}")]
		public ActionResult Patch(int id, [FromBody] TodoItemDto value)
		{
			var todoItem = todoItems.FirstOrDefault(x => x.Id == id);
			if (value.Title != null) todoItem.Title = value.Title;
			if(value.Completed != null) todoItem.Completed = value.Completed;
			if (value.Order != null) todoItem.Order = value.Order;

			return this.Ok(todoItem);
		}

		// DELETE api/values/5
		[HttpDelete()]
		public ActionResult Delete()
		{
			todoItems.Clear();
			return this.Ok();
		}

		// DELETE api/values/5
		[HttpDelete("{id}")]
		public ActionResult Delete(int id)
		{
			var todoItem = todoItems.FirstOrDefault(x => x.Id == id);
			todoItems.Remove(todoItem);
			return this.Ok();
		}
	}
}
