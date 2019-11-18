using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using TodoAppDdd.App.Common;
using TodoAppDdd.App.Contracts.Command;
using TodoAppDdd.App.Contracts.Command.Handler;
using TodoAppDdd.App.Contracts.Query;
using TodoAppDdd.Domain.Aggregate;
using TodoAppDdd.Domain.Aggregate.Exceptions;
using TodoAppDdd.Persistence;

namespace TodoAppDdd.App.Command
{
	public class UpdateAllFieldsOfTodoItemCommandHandler : IUpdateAllFieldsOfTodoItemCommandHandler
	{
		private readonly ITodoRepository _repository;
		private readonly ITodoItemMapper _mapper;

		public UpdateAllFieldsOfTodoItemCommandHandler(ITodoRepository repository, ITodoItemMapper mapper)
		{
			this._repository = repository;
			this._mapper = mapper;
		}

		public TodoItemDto Handle(UpdateAllFieldsOfTodoItemCommand command)
		{
			var todoItem = this._repository.GetTodo(command.Id);

			if (todoItem == null)
			{
				throw new AggregateNotFoundException();
			}

			if (command.Title != null)
			{
				todoItem.UpdateTitle(command.Title);
			}
			if (command.Order != null)
			{
				todoItem.UpdateOrder(command.Order.Value);
			}
			if (command.Completed != null)
			{
				if (command.Completed.Value)
				{
					try
					{
						todoItem.Finish();
					}
					catch (TodoItemAlreadyCompletedException e)
					{
						Debug.WriteLine("This TodoItem has already been finished.");
					}
				}
				else
				{
					try
					{
						todoItem.Reset();
					}
					catch(TodoItemNotResettableException ex)
					{
						Debug.WriteLine("TodoItem is not resettable.");
					}
				}
			}

			this._repository.SaveState(todoItem);

			return this._mapper.Map(todoItem);
		}
	}

	public class AggregateNotFoundException : Exception
	{
	}
}
