using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TodoAppDdd.Persistence
{
	public class StoreContextFactory : IDesignTimeDbContextFactory<TodoAppDddContext>
	{
		public TodoAppDddContext CreateDbContext(string[] args)
		{
			var configuration = new ConfigurationBuilder()
				.SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
				.AddJsonFile("appsettings.json", optional: false)
				.Build();

			var builder = new DbContextOptionsBuilder<TodoAppDddContext>();
			var connectionString = configuration.GetConnectionString("DefaultConnection");
			Debug.WriteLine("ConnS: "+ connectionString);

			builder.UseSqlServer(connectionString);

			return new TodoAppDddContext(builder.Options);
		}
	}
}
