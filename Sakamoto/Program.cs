using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Net;

namespace Sakamoto
{
	public class Program
	{
		public static void Main(string[] args)
		{
			Manager.ChatManager.Init();
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(builder =>
				{
					builder.UseKestrel(config =>
					{
						config.Limits.MaxRequestBodySize = null;
						config.Listen(IPAddress.Loopback, 20002);
					});
					builder.UseStartup<Startup>();
				})
				.StartAsync();
		}
	}
}
