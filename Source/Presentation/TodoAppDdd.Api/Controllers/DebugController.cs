using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TodoAppDdd.App.Contracts.Command;
using TodoAppDdd.App.Contracts.Command.Handler;
using TodoAppDdd.App.Contracts.Query;
using TodoAppDdd.App.Contracts.Query.Handler;
using TodoAppDdd.Persistence;
using TodoAppDdd.Persistence.EventStore;

namespace TodoAppDdd.Api.Controllers
{

	[Route("api/[controller]")]
	[ApiController]
	public class DebugController : ControllerBase
	{
		private readonly IEventStore _eventStore;

		public DebugController(IEventStore eventStore)
		{
			this._eventStore = eventStore;
		}

		// GET api/values
		[HttpGet("/GetRawEvents")]
		public ActionResult<IEnumerable<TodoItemDto>> Get()
		{
			var lines = _eventStore.OutputRawEvents();

			return this.Ok(lines.Reverse().Skip(0).Take(10));
		}
		
		// GET api/values
		[HttpPost("/Delete")]
		public ActionResult<IEnumerable<TodoItemDto>> Delete()
		{
			_eventStore.DropAllEvents();

			return this.Ok();
		}
	}
}
