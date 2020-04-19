using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace Sakamoto
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{

			services.AddControllers();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
		{


			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler(errorApp =>
				{
					errorApp.Run(async context =>
					{
						context.Response.StatusCode = 500;
						context.Response.ContentType = "text/plain";
						var exceptionHandlerPathFeature =
							context.Features.Get<IExceptionHandlerPathFeature>();
						await context.Response.WriteAsync("Error Thrown.\n" + exceptionHandlerPathFeature?.Error.StackTrace);
					});
				});
			}
			app.UseStatusCodePages(async context =>
			{
				Console.WriteLine(context.HttpContext.Response.ContentType);
				context.HttpContext.Response.ContentType = "text/plain";
				switch (context.HttpContext.Response.StatusCode)
				{
					case 404:
						await context.HttpContext.Response.WriteAsync("Sakamoto was smart, but he don't know about this page. (404)");
						return;
					default:
						await context.HttpContext.Response.WriteAsync($"Something wrong. {context.HttpContext.Response.StatusCode}");
						return;
				}
			});
			app.UseRouting();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
