using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TodoAppDdd.App.Contracts.Query;
using Microsoft.AspNetCore.Mvc;
using TodoAppDdd.App.Contracts.Query.Handler;
using TodoAppDdd.Web.Models;

namespace TodoAppDdd.Web.Controllers
{
	public class HomeController : Controller
	{
		private readonly IGetTodoItemsQueryHandler _getTodoItemsQueryHandler;

		public HomeController(IGetTodoItemsQueryHandler getTodoItemsQueryHandler)
		{
			this._getTodoItemsQueryHandler = getTodoItemsQueryHandler;
		}

		public async Task<IActionResult> Index([FromQuery] int? goBack)
		{
            IEnumerable<TodoItemDto> todoItems = await this._getTodoItemsQueryHandler.Handle(new GetTodoItemsQuery() { GoBackSeconds = goBack});
			return View(todoItems);
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
