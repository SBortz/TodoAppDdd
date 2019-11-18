using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace TodoAppDdd.Web
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateWebHostBuilder(args).Build().Run();
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseEnvironment(GetEnv().ToString())
				.UseStartup<Startup>();

		private static HostingEnv GetEnv()
		{
#if DEVINMEM
			return HostingEnv.DevInMem;
#elif DEVTEXTSTORE
			return HostingEnv.DevTextStore;
#else
			return HostingEnv.OnAzure;
#endif
		}

		public enum HostingEnv
		{
			DevInMem,
			DevTextStore,
			OnAzure
		}
	}
}
