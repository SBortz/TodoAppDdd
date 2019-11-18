using Microsoft.AspNetCore.Hosting;

namespace TodoAppDdd.Api
{
	public static class HostingEnvironmentExtensions
	{
		public static bool IsDevInMem(this IHostingEnvironment env)
		{
			return env.EnvironmentName == Program.HostingEnv.DevInMem.ToString();
		}

		public static bool IsDevTextStore(this IHostingEnvironment env)
		{
			return env.EnvironmentName == Program.HostingEnv.DevTextStore.ToString();
		}

		public static bool IsOnAzure(this IHostingEnvironment env)
		{
			return env.EnvironmentName == Program.HostingEnv.OnAzure.ToString();
		}
	}
}