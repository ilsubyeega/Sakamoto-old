using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Sakamoto.Database;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

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
			services.AddLogging(l =>
			{
				l.ClearProviders();
				// Debug
				l.AddFilter("Microsoft.AspNetCore.SignalR", LogLevel.Debug);
				l.AddFilter("Microsoft.AspNetCore.Http.Connections", LogLevel.Debug);
				l.AddConsole();
			});
			services.AddDbContext<MariaDBContext>(
				options =>
				{
					options.UseMySql("server=localhost;port=3306;database=keesu;uid=dev;password=dev", MariaDbServerVersion.LatestSupportedServerVersion,
					   mySqlOptionsAction: sqlOptions =>
					   {
						   sqlOptions.EnableRetryOnFailure(
							  maxRetryCount: 20,
							  maxRetryDelay: TimeSpan.FromSeconds(10),
							  errorNumbersToAdd: null);
					   });
				}
			);

			services.AddAuthentication(cfg =>
			{
				cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(opt =>
			{
				opt.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateAudience = false,
					ValidateIssuer = false,
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(Common.GetKey()),
					ValidateLifetime = true,
					ClockSkew = TimeSpan.FromMinutes(0.5)
				};

				opt.Events = new JwtBearerEvents
				{
					OnTokenValidated = async context =>
					{
						Console.WriteLine("Log");
						var token = (JwtSecurityToken)context.SecurityToken;
						var obj = (object)null;
						if (obj == null)
						{
							context.Response.Clear();
							context.Response.StatusCode = 401;
							var res = JsonConvert.SerializeObject(new { a = "owo" });
							await context.Response.BodyWriter.WriteAsync(Encoding.UTF8.GetBytes(res));
						}
						else
						{
							Console.WriteLine(token.RawData);
							Console.WriteLine(obj);

							context.Success();
						}
					},
					OnAuthenticationFailed = async context =>
					{
						context.Response.Clear();
						context.Response.StatusCode = 401;
						var res = JsonConvert.SerializeObject(new { a = "auth failed" });
						await context.Response.BodyWriter.WriteAsync(Encoding.UTF8.GetBytes(res));
					}
				};

			});


			services.AddAuthorization();

			services.Configure<FormOptions>(
				x =>
				{
					x.ValueLengthLimit = int.MaxValue;
					x.MultipartBodyLengthLimit = int.MaxValue;
					x.MemoryBufferThreshold = int.MaxValue;
					x.BufferBodyLengthLimit = int.MaxValue;
					x.MultipartBoundaryLengthLimit = int.MaxValue;
					x.MultipartHeadersLengthLimit = int.MaxValue;
				}
			);

			services.AddCors(options =>
			{
				options.AddPolicy("CorsPolicy",
					builder => builder
						.AllowAnyMethod()
						.AllowCredentials()
						.SetIsOriginAllowed((host) => true)
						.AllowAnyHeader());
			});
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Sakamoto", Version = "v1" });
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sakamoto"));
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseWebSockets();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
