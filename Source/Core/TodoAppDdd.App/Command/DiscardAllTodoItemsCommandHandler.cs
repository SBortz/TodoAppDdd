﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
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

		public DiscardAllTodoItemsCommandHandler(ITodoRepository todoRepository)
		{
			this._todoRepository = todoRepository;
		}

		public void Handle(DiscardAllTodoItemsCommand command)
		{
			var allTodos = this._todoRepository.GetAllTodos();

			foreach (var todoItem in allTodos)
			{
				try
				{
					todoItem.Discard();
					this._todoRepository.SaveState(todoItem);
				}
				catch(TodoItemAlreadyDiscardedException ex)
				{
					Debug.WriteLine("This TodoItem has already been discarded.");
				}
			}
		}
	}
}
