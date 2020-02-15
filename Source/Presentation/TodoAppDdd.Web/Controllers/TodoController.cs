using System.Threading.Tasks;
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
		public async Task<IActionResult> Create([FromBody] CreateTodoItemCommand cmd)
		{
			await this._createTodoItemItemCommandHandler.Handle(cmd);

			return this.Ok();
		}

		public async Task<IActionResult> Discard([FromBody] DiscardTodoItemCommand cmd)
		{
			await this._discardTodoItemCommandHandler.Handle(cmd);

			return this.Ok();
		}

		public async Task<IActionResult> Finish([FromBody] FinishTodoItemCommand cmd)
		{
            await this._finishTodoItemCommandHandler.Handle(cmd);

			return this.Ok();
		}

		public async Task<IActionResult> Reset([FromBody] ResetTodoItemCommand cmd)
		{
            await this._resetTodoItemCommandHandler.Handle(cmd);

			return this.Ok();
		}

		public async Task<IActionResult> Restore([FromBody] RestoreDiscardedTodoItemsCommand cmd)
		{
            await this._restoreDiscardedTodoItemsCommandHandler.Handle(cmd);

			return this.Ok();
		}

		public async Task<IActionResult> DiscardAll([FromBody] DiscardAllTodoItemsCommand command)
		{
            await this._discardAllTodoItemsCommandHandler.Handle(command);

			return this.Ok();
		}
	}
}