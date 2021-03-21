using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Sakamoto.Helper.Config;
using static Microsoft.AspNetCore.Hosting.WebHostBuilderKestrelExtensions;

namespace Sakamoto
{
	public class Program
	{
		public static void Main(string[] args)
		{
			// Initial
			ConfigParser.Parse();

			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)

				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
					webBuilder.UseUrls("https://localhost:23212/", "http://localhost:23211");
					webBuilder.UseKestrel(options => options.AddServerHeader = false);
				});
	}
}
