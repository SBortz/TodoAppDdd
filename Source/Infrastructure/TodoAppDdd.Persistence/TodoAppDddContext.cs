using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TodoAppDdd.Domain.ReadModel;

namespace TodoAppDdd.Persistence
{
	public class TodoAppDddContext : DbContext
	{
		public TodoAppDddContext(DbContextOptions<TodoAppDddContext> options) : base(options)
		{
		}

		public DbSet<TodoItemReadModel> TodoItem { get; set; }
	}
}
