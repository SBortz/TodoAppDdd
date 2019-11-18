using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using TodoAppDdd.App.Command;
using TodoAppDdd.App.Contracts.Command;
using TodoAppDdd.App.Contracts.Query;
using TodoAppDdd.App.Query;
using TodoAppDdd.Persistence;
using Microsoft.Extensions.DependencyInjection;
using TodoAppDdd.App.Common;
using TodoAppDdd.App.Contracts.Command.Handler;
using TodoAppDdd.App.Contracts.Query.Handler;
using TodoAppDdd.Persistence.EventStore;

namespace TodoAppDdd.Bootstrapper
{
	public static class ServiceCollectionExtensions
	{
		public static void AddApplication(this IServiceCollection serviceCollection)
		{
			serviceCollection.AddTransient<ICreateTodoItemCommandHandler, CreateTodoItemCommandHandler>();
			serviceCollection.AddTransient<IDiscardTodoItemCommandHandler, DiscardTodoItemCommandHandler>();
			serviceCollection.AddTransient<IFinishTodoItemCommandHandler, FinishTodoItemCommandHandler>();
			serviceCollection.AddTransient<IResetTodoItemCommandHandler, ResetTodoItemCommandHandler>();
			serviceCollection.AddTransient<IGetTodoItemsQueryHandler, GetTodoItemsQueryHandler>();
			serviceCollection.AddTransient<IUpdateAllFieldsOfTodoItemCommandHandler, UpdateAllFieldsOfTodoItemCommandHandler>();
			serviceCollection.AddTransient<IDiscardAllTodoItemsCommandHandler, DiscardAllTodoItemsCommandHandler>();
			serviceCollection.AddTransient<IRestoreDiscardedTodoItemsCommandHandler, RestoreDiscardedTodoItemsCommandHandler>();
			serviceCollection.AddTransient<IGetTodoItemQueryHandler, GetTodoItemQueryHandler>();
			serviceCollection.AddTransient<ITodoRepository, TodoRepository>();
			serviceCollection.AddTransient<ITodoItemMapper, TodoItemMapper>();
		}

		public static void AddTextEventStore(this IServiceCollection serviceCollection)
		{
			serviceCollection.AddSingleton<IEventStore, TextFileEventStore>();
		}

		public static void AddInMemoryEventStore(this IServiceCollection serviceCollection)
		{
			serviceCollection.AddSingleton<IEventStore, InMemoryEventStore>();
		}
	}
}
