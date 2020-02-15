using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TodoAppDdd.App.Contracts.Command;
using TodoAppDdd.App.Contracts.Command.Handler;
using TodoAppDdd.App.Contracts.Query;
using TodoAppDdd.App.Contracts.Query.Handler;

namespace TodoAppDdd.Api.Controllers
{

	[Route("api/[controller]")]
	[ApiController]
	public class TodoController : ControllerBase
	{
		private readonly ICreateTodoItemCommandHandler _createTodoItemItemCommandHandler;
		private readonly IDiscardTodoItemCommandHandler _discardTodoItemCommandHandler;
		private readonly IDiscardAllTodoItemsCommandHandler _discardAllTodoItemsCommandHandler;
		private readonly IFinishTodoItemCommandHandler _finishTodoItemCommandHandler;
		private readonly IResetTodoItemCommandHandler _resetTodoItemCommandHandler;
		private readonly IUpdateAllFieldsOfTodoItemCommandHandler _updateAllFieldsOfTodoItemCommandHandler;
		private readonly IGetTodoItemsQueryHandler _getTodoItemsQueryHandler;
		private readonly IGetTodoItemQueryHandler _getTodoItemQueryHandler;

		public TodoController(ICreateTodoItemCommandHandler createTodoItemItemCommandHandler, IDiscardTodoItemCommandHandler discardTodoItemCommandHandler, IFinishTodoItemCommandHandler finishTodoItemCommandHandler, IResetTodoItemCommandHandler resetTodoItemCommandHandler, IUpdateAllFieldsOfTodoItemCommandHandler updateAllFieldsOfTodoItemCommandHandler, IGetTodoItemsQueryHandler getTodoItemsQueryHandler, IGetTodoItemQueryHandler getTodoItemQueryHandler, IDiscardAllTodoItemsCommandHandler _discardAllTodoItemsCommandHandler)
		{
			this._createTodoItemItemCommandHandler = createTodoItemItemCommandHandler;
			this._discardTodoItemCommandHandler = discardTodoItemCommandHandler;
			this._finishTodoItemCommandHandler = finishTodoItemCommandHandler;
			this._resetTodoItemCommandHandler = resetTodoItemCommandHandler;
			this._updateAllFieldsOfTodoItemCommandHandler = updateAllFieldsOfTodoItemCommandHandler;
			this._getTodoItemsQueryHandler = getTodoItemsQueryHandler;
			this._getTodoItemQueryHandler = getTodoItemQueryHandler;
			this._discardAllTodoItemsCommandHandler = _discardAllTodoItemsCommandHandler;
		}

		// GET api/values
		[HttpGet]
		public async Task<ActionResult<IEnumerable<TodoItemDto>>> Get()
		{
			var items = await this._getTodoItemsQueryHandler.Handle(new GetTodoItemsQuery());

			return this.Ok(items);
		}

		// GET api/values/5
		[HttpGet("{id}")]
		public async Task<ActionResult<TodoItemDto>> Get(string id)
		{
			var item = await this._getTodoItemQueryHandler.Handle(new GetTodoItemQuery() { Id = id });

			return this.Ok(item);
		}

		// POST api/values
		[HttpPost]
		public async Task<ActionResult> Post([FromBody] TodoItemDto value)
		{
			var item = this._createTodoItemItemCommandHandler.Handle(
				new CreateTodoItemCommand()
				{
					Title = value.Title,
					Order = value.Order,
				} );

			return this.Ok(item);
		}

		// PUT api/values/5
		[HttpPatch("{id}")]
		public async Task<ActionResult> Patch(string id, [FromBody] TodoItemDto value)
		{
			var item = await this._updateAllFieldsOfTodoItemCommandHandler.Handle(
				new UpdateAllFieldsOfTodoItemCommand()
				{
					Id = id,
					Title = value.Title,
					Order = value.Order,
					Completed = value.Completed
				});

			return this.Ok(item);
		}

		[HttpDelete()]
		public async Task<ActionResult> Delete()
		{
            await this._discardAllTodoItemsCommandHandler.Handle(new DiscardAllTodoItemsCommand());

			return this.Ok();
		}

		// DELETE api/values/5
		[HttpDelete("{id}")]
		public async Task<ActionResult> Delete(string id)
		{
            await this._discardTodoItemCommandHandler.Handle(new DiscardTodoItemCommand() { Id = id });

			return this.Ok();
		}
	}
}
