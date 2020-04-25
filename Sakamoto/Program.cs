using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Net;

namespace Sakamoto
{
	public class Program
	{
		public static void Main(string[] args)
		{
			Initalize.Init();

			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(builder =>
				{
					builder.UseKestrel(config =>
					{
						config.Limits.MaxRequestBodySize = null;
					});
					builder.UseStartup<Startup>();
					builder.UseUrls("http://localhost:20002");
				})
				.StartAsync();
		}
	}
}
