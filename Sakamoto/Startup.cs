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
using Newtonsoft.Json;
using Sakamoto.Database;
using Sakamoto.Database.Models;
using Sakamoto.Helper.Config;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
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
			
			services.AddControllers().AddNewtonsoftJson();
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
					options.UseMySql(ConfigParser.Config.Database.Url, MariaDbServerVersion.LatestSupportedServerVersion,
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
						var token = (JwtSecurityToken)context.SecurityToken;
						var id = token.Claims.FirstOrDefault(claim => claim.Type == "token").Value;
						var dbcontext = context.HttpContext.RequestServices.GetRequiredService<MariaDBContext>();
						var timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
						var obj = await dbcontext.AccessTokens.FirstOrDefaultAsync(ac => ac.Id == id && ac.Revoked == false && ac.ExpiresAt > timestamp);
						DBUser user = null;
						if (obj != null) user = dbcontext.Users.Where(a => a.Id == obj.UserId).FirstOrDefault();
						if (obj == null || user == null)
						{
							context.Fail("auth failed");
						}
						else
						{
							context.HttpContext.Items["user"] = user;
							context.HttpContext.Items["access"] = obj;
							context.Success();
						}
					},
					OnAuthenticationFailed = async context =>
					{
						context.Response.OnStarting(async () =>
						{
							context.Response.StatusCode = 401;
							context.Response.ContentType = "text/plain";
							var res = JsonConvert.SerializeObject(
								new {
									auth = "auth failed",
									exception = new
									{
										message = context.Exception.Message
									}
								});
							await context.Response.BodyWriter.WriteAsync(Encoding.UTF8.GetBytes(res));
						});
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
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

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
