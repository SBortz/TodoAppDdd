using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TodoAppDdd.App.Common;
using TodoAppDdd.App.Contracts.Command;
using TodoAppDdd.App.Contracts.Query;
using TodoAppDdd.Domain.Aggregate;
using TodoAppDdd.Domain.DDDBase;
using TodoAppDdd.Persistence;

namespace TodoAppDdd.App.Command
{
	public class CreateTodoItemCommandHandler : ICreateTodoItemCommandHandler
	{
		private readonly ITodoRepository _todoRepository;
		private readonly ITodoItemMapper _mapper;
		private readonly ITodoItemReadModelRepository _todoItemReadModelRepository;

		public CreateTodoItemCommandHandler(ITodoRepository todoRepository, ITodoItemMapper mapper, ITodoItemReadModelRepository todoItemReadModelRepository)
		{
			this._todoRepository = todoRepository;
			this._mapper = mapper;
			this._todoItemReadModelRepository = todoItemReadModelRepository;
		}

		public async Task<TodoItemDto> Handle(CreateTodoItemCommand itemCommand)
		{
			var todoItem = new TodoItem(itemCommand.Title, itemCommand.Order);

			var happenedDomainEvents = todoItem.DomainEvents;


			await this._todoRepository.SaveState(todoItem);
			// this.eventPublisher.Publish(happenedDomainEvents);

			
//
//			this._todoItemReadModelRepository.InsertOrUpdateFromTodoItem(todoItem);
			return this._mapper.Map(todoItem);
		}
	}
}
