using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.EntityFrameworkCore;
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
			serviceCollection.AddTransient<IUpdateAllFieldsOfTodoItemCommandHandler, UpdateAllFieldsOfTodoItemCommandHandler>();
			serviceCollection.AddTransient<IDiscardAllTodoItemsCommandHandler, DiscardAllTodoItemsCommandHandler>();
			serviceCollection.AddTransient<IRestoreDiscardedTodoItemsCommandHandler, RestoreDiscardedTodoItemsCommandHandler>();
			serviceCollection.AddTransient<ITodoRepository, TodoRepository>();
			serviceCollection.AddTransient<ITodoItemMapper, TodoItemMapper>();
		}

		public static void AddQueryHandler(this IServiceCollection serviceCollection)
		{
			// EventSourcing as Query datasource for querying data
			serviceCollection.AddTransient<IGetTodoItemsQueryHandler, GetTodoItemsQueryHandler>();
			serviceCollection.AddTransient<IGetTodoItemQueryHandler, GetTodoItemQueryHandler>();
			serviceCollection.AddTransient<ITodoItemReadModelRepository, FakeTodoItemReadModelRepository>();

		}

		public static void AddReadModelQueryHandler(this IServiceCollection serviceCollection)
		{
			// ReadModel: SQL Table as data source for querying data
			serviceCollection.AddTransient<IGetTodoItemsQueryHandler, GetTodoItemsReadModelQueryHandler>();
			serviceCollection.AddTransient<IGetTodoItemQueryHandler, GetTodoItemReadModelQueryHandler>();
			serviceCollection.AddTransient<ITodoItemReadModelRepository, TodoItemReadModelRepository>();
		}

		public static void AddTextEventStore(this IServiceCollection serviceCollection)
		{
			serviceCollection.AddSingleton<IEventStore, TextFileEventStore>();
		}

		public static void AddInMemoryEventStore(this IServiceCollection serviceCollection)
		{
			serviceCollection.AddSingleton<IEventStore, InMemoryEventStore>();
		}

		public static void AddTodoAppDddContext(this IServiceCollection services)
		{
			services.AddDbContext<TodoAppDddContext>(options =>
			{
				options.UseInMemoryDatabase("TodoAppDdd");
			});
		}
	}
}
