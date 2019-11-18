using TodoAppDdd.App.Contracts.Command;
using Microsoft.AspNetCore.Mvc;
using TodoAppDdd.App.Contracts.Command.Handler;

namespace TodoAppDdd.Web.Controllers
{
	public class TodoController : Controller
	{
		private readonly ICreateTodoItemCommandHandler _createTodoItemItemCommandHandler;
		private readonly IDiscardTodoItemCommandHandler _discardTodoItemCommandHandler;
		private readonly IFinishTodoItemCommandHandler _finishTodoItemCommandHandler;
		private readonly IResetTodoItemCommandHandler _resetTodoItemCommandHandler;
		private readonly IRestoreDiscardedTodoItemsCommandHandler _restoreDiscardedTodoItemsCommandHandler;
		private readonly IDiscardAllTodoItemsCommandHandler _discardAllTodoItemsCommandHandler;

		public TodoController(ICreateTodoItemCommandHandler createTodoItemItemCommandHandler, IDiscardTodoItemCommandHandler discardTodoItemCommandHandler, IFinishTodoItemCommandHandler finishTodoItemCommandHandler, IResetTodoItemCommandHandler resetTodoItemCommandHandler, IRestoreDiscardedTodoItemsCommandHandler restoreDiscardedTodoItemsCommandHandler, IDiscardAllTodoItemsCommandHandler discardAllTodoItemsCommandHandler)
		{
			this._createTodoItemItemCommandHandler = createTodoItemItemCommandHandler;
			this._discardTodoItemCommandHandler = discardTodoItemCommandHandler;
			this._finishTodoItemCommandHandler = finishTodoItemCommandHandler;
			this._resetTodoItemCommandHandler = resetTodoItemCommandHandler;
			this._restoreDiscardedTodoItemsCommandHandler = restoreDiscardedTodoItemsCommandHandler;
			this._discardAllTodoItemsCommandHandler = discardAllTodoItemsCommandHandler;
		}

		[HttpPost]
		public IActionResult Create([FromBody] CreateTodoItemCommand cmd)
		{
			this._createTodoItemItemCommandHandler.Handle(cmd);

			return this.Ok();
		}

		public IActionResult Discard([FromBody] DiscardTodoItemCommand cmd)
		{
			this._discardTodoItemCommandHandler.Handle(cmd);

			return this.Ok();
		}

		public IActionResult Finish([FromBody] FinishTodoItemCommand cmd)
		{
			this._finishTodoItemCommandHandler.Handle(cmd);

			return this.Ok();
		}

		public IActionResult Reset([FromBody] ResetTodoItemCommand cmd)
		{
			this._resetTodoItemCommandHandler.Handle(cmd);

			return this.Ok();
		}

		public IActionResult Restore([FromBody] RestoreDiscardedTodoItemsCommand cmd)
		{
			this._restoreDiscardedTodoItemsCommandHandler.Handle(cmd);

			return this.Ok();
		}

		public IActionResult DiscardAll([FromBody] DiscardAllTodoItemsCommand command)
		{
			this._discardAllTodoItemsCommandHandler.Handle(command);

			return this.Ok();
		}
	}
}