using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
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
		private readonly ITodoItemReadModelRepository _readModelRepository;

		public UpdateAllFieldsOfTodoItemCommandHandler(ITodoRepository repository, ITodoItemMapper mapper, ITodoItemReadModelRepository readModelRepository)
		{
			this._repository = repository;
			this._mapper = mapper;
			this._readModelRepository = readModelRepository;
		}

		public async Task<TodoItemDto> Handle(UpdateAllFieldsOfTodoItemCommand command)
		{
			var todoItem = await this._repository.GetTodoAsync(command.Id);

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

			await this._repository.SaveState(todoItem);
			this._readModelRepository.InsertOrUpdateFromTodoItem(todoItem);

			return this._mapper.Map(todoItem);
		}
	}

	public class AggregateNotFoundException : Exception
	{
	}
}
