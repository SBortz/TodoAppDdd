using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using TodoAppDdd.Api.Filter;
using TodoAppDdd.Bootstrapper;
using TodoAppDdd.Persistence;

namespace TodoAppDdd.Api
{
	public class Startup
	{
		private readonly IHostingEnvironment env;

		public Startup(IConfiguration configuration, IHostingEnvironment env)
		{
			Configuration = configuration;
			this.env = env;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

			services.AddApplication();
			if (this.env.IsDevInMem())
			{
				services.AddInMemoryEventStore();
			}
			else if(this.env.IsDevTextStore())
			{
				services.AddTextEventStore();
			}
			else
			{
				services.AddInMemoryEventStore();
			}

			services.Configure<MvcOptions>(options =>
			{
				options.Filters.Add(new CorsAuthorizationFilterFactory("AllowMyOrigin"));
				options.Filters.Add<ApiExceptionFilterAttribute>();
			});
			services.AddCors(options =>
			{
				options.AddPolicy("AllowMyOrigin",
					builder => builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod().Build());
			});

			services.AddSwaggerGen(options =>
			{
				options.SwaggerDoc("v1", new Info { Title = "TodoAppDdd API", Version = "v1" });
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			app.UseCors("AllowMyOrigin");
			app.UseDeveloperExceptionPage();

			app.UseSwagger();
			app.UseSwaggerUI(options =>
			{
				options.SwaggerEndpoint("/swagger/v1/swagger.json", "TodoAppDdd API");
			});

			app.UseHttpsRedirection();
			app.UseMvc();
		}
	}
}
