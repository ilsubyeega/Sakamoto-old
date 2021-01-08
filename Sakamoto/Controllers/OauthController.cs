using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sakamoto.Database;
using Sakamoto.Database.Models.OAuth;
using Sakamoto.Util;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace Sakamoto.Controllers
{
	[Route("oauth")]
	[ApiController]
	public class OauthController : ControllerBase
	{
		private readonly MariaDBContext _dbcontext;
		public OauthController(MariaDBContext mariaDBContext) { _dbcontext = mariaDBContext; }

		[HttpGet("")]
		[AllowAnonymous]
		public IActionResult Index()
		{
			return Ok("o/");
		}
		[HttpGet("testauth")]
		[Authorize]
		public IActionResult TestAuth()
		{
			return Ok("o/");
		}
		[HttpPost("token")]
		[AllowAnonymous]
		public IActionResult GetToken()
		{
			// grant_type client_id client_secret scope (code / refresh_token / redirect_uri) or (username, password)
			var form = Request.Form;
			try
			{
				var grant_type = GetValue(form, "grant_type");
				var client_id_tmp = GetValue(form, "client_id");
				var client_secret = GetValue(form, "client_secret");
				var scope = GetValue(form, "scope");


				long client_id;
				try
				{
					if (grant_type is null || client_id_tmp is null || client_secret is null || scope is null) return StatusCode(401, new { error = "Wrong Body." });
					client_id = uint.Parse(client_id_tmp);
				}
				catch
				{
					return StatusCode(401, new { error = "Wrong Body." });
				}
				
				var client = _dbcontext.Clients.FirstOrDefault(c => c.Id == client_id && c.Secret == client_secret && c.Revoked == false);
				Console.WriteLine("A");
				if (client == null) return StatusCode(401, new { error = "Client not found." });

				switch (grant_type)
				{
					case "code":
						return StatusCode(500, "Not implemented");
					case "authorization_code":
						return StatusCode(500, "Not implemented");
					case "refresh_token":
						{
							var refresh_token_tmp = GetValue(form, "refresh_token");
							if (refresh_token_tmp is null) return StatusCode(401, new { error = "Wrong Body." });
							
							var handler = new JwtSecurityTokenHandler();
							var tokens = handler.ReadJwtToken(refresh_token_tmp);

							var refresh_claim = tokens.Claims.FirstOrDefault(a => a.Type == "token");
							if (refresh_claim is null) return StatusCode(401, new { error = "Wrong Validated Token." });
							var refresh_token = refresh_claim.Value;

							var timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
							var refresh = _dbcontext.RefreshTokens.FirstOrDefault(a => a.Id == refresh_token && a.Revoked == false && a.ExpiresAt > timestamp);
							if (refresh is null)
								return StatusCode(401, "Wrong token.");
							var access = _dbcontext.AccessTokens.FirstOrDefault(a => a.Id == refresh.AccessToken); // this should be setted.

							if (access.ClientId != client_id)
								return StatusCode(401, "Wrong client.");

							var randomAccess = GenerateRandomUntilNotExists(false);
							var randomRefresh = GenerateRandomUntilNotExists(true);

							var accesstoken = JwtUtil.GenerateToken(randomAccess);
							var refreshtoken = JwtUtil.GenerateRefreshToken(randomRefresh);

							access.Revoked = true;
							refresh.Revoked = true;
							_dbcontext.AccessTokens.Add(new DBAccessToken
							{
								Id = randomAccess,
								UserId = access.UserId,
								ClientId = client.Id,
								Scopes = "*",
								Revoked = false,
								CreatedAt = timestamp,
								ExpiresAt = DateTimeOffset.Now.AddDays(1).ToUnixTimeSeconds()
							});
							_dbcontext.RefreshTokens.Add(new DBRefreshToken
							{
								Id = randomRefresh,
								AccessToken = randomAccess,
								Revoked = false,
								CreatedAt = timestamp,
								ExpiresAt = DateTimeOffset.Now.AddDays(30).ToUnixTimeSeconds()
							});

							_dbcontext.SaveChanges();
							return Ok(new
							{
								token_type = "Bearer",
								access_token = accesstoken,
								refresh_token = refreshtoken,
								expires_in = 86400
							});
						}
					case "password":
						{
							if (!client.IsPasswordClient) return StatusCode(401, new { error = "This client is not password client." });
							var username = GetValue(form, "username");
							var password = GetValue(form, "password");

							if (username is null || password is null) return StatusCode(401, new { error = "Wrong Body." });

							var password_hashed = CryptoUtil.GetMD5Hash(password);
							var user = _dbcontext.Users.FirstOrDefault(obj => obj.UserName.ToLower() == username);
							if (!CryptoUtil.CheckScrypt(password_hashed, user.Password)) return StatusCode(401, new { error = "Wrong Password." });

							// Generate
							var randomAccess = GenerateRandomUntilNotExists(false);
							var randomRefresh = GenerateRandomUntilNotExists(true);



							var access = JwtUtil.GenerateToken(randomAccess);
							var refresh = JwtUtil.GenerateRefreshToken(randomRefresh);

							var timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
							_dbcontext.AccessTokens.Add(new DBAccessToken
							{
								Id = randomAccess,
								UserId = user.Id,
								ClientId = client.Id,
								Scopes = "*",
								Revoked = false,
								CreatedAt = timestamp,
								ExpiresAt = DateTimeOffset.Now.AddDays(1).ToUnixTimeSeconds()
							});
							_dbcontext.RefreshTokens.Add(new DBRefreshToken
							{
								Id = randomRefresh,
								AccessToken = randomAccess,
								Revoked = false,
								CreatedAt = timestamp,
								ExpiresAt = DateTimeOffset.Now.AddDays(30).ToUnixTimeSeconds()
							});
							_dbcontext.SaveChanges();
							return Ok(new
							{
								token_type = "Bearer",
								access_token = access,
								refresh_token = refresh,
								expires_in = 86400
							});
						}


					case "client_credentials":
						return StatusCode(500, "Not implemented");
					default:
						return StatusCode(500, "No method found.");
				}

			} catch (Exception e)
			{
				Console.WriteLine(e.Message);
				Console.WriteLine(e.StackTrace);
				return StatusCode(500, "Something Happened... Sent error logs to developer.");
			}
			
			return Ok("owo!");
		}

		private string GenerateRandomUntilNotExists(bool isRefresh)
		{
			string random = JwtUtil.RandomString();
			if (isRefresh)
			{
				while (_dbcontext.RefreshTokens.Any(a => a.Id == random))
					random = JwtUtil.RandomString();
			} else
			{
				while (_dbcontext.AccessTokens.Any(a => a.Id == random))
					random = JwtUtil.RandomString();
			}
			return random ?? null;
		}
		private static string GetValue(IFormCollection form, string key) => form.ContainsKey(key) ? form[key].ToString() : null;

	}
}
