using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using TodoAppDdd.App.Contracts.Command;
using TodoAppDdd.App.Contracts.Command.Handler;
using TodoAppDdd.Domain.Aggregate;
using TodoAppDdd.Domain.Aggregate.Exceptions;
using TodoAppDdd.Persistence;

namespace TodoAppDdd.App.Command
{
	public class DiscardAllTodoItemsCommandHandler : IDiscardAllTodoItemsCommandHandler
	{
		private readonly ITodoRepository _todoRepository;
		private readonly ITodoItemReadModelRepository _readModelRepository;

		public DiscardAllTodoItemsCommandHandler(ITodoRepository todoRepository, ITodoItemReadModelRepository readModelRepository)
		{
			this._todoRepository = todoRepository;
			this._readModelRepository = readModelRepository;
		}

		public async Task Handle(DiscardAllTodoItemsCommand command)
		{
			var allTodos = await this._todoRepository.GetAllTodos();
			
			foreach (var todoItem in allTodos)
			{
				try
				{
					todoItem.Discard();
					await this._todoRepository.SaveState(todoItem);
					this._readModelRepository.InsertOrUpdateFromTodoItem(todoItem);
				}
				catch(TodoItemAlreadyDiscardedException ex)
				{
					Debug.WriteLine("This TodoItem has already been discarded.");
				}
			}
		}
	}
}
