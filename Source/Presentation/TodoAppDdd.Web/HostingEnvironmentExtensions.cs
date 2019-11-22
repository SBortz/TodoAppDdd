using Microsoft.AspNetCore.Hosting;

namespace TodoAppDdd.Web
{
	public static class HostingEnvironmentExtensions
	{
		public static bool IsOnAzure(this IHostingEnvironment env)
		{
			return env.EnvironmentName == Program.HostingEnv.OnAzure.ToString();
		}
	}
}