using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Sakamoto.Util
{
	public static class JwtUtil
	{

		public static string GenerateToken(string value, Int32 expiresec = 86400)
		{
			var key = new SymmetricSecurityKey(Common.GetKey());
			var jwt = new JwtSecurityToken(issuer: "Sakamoto",
				claims: new Claim[] {
					new Claim("token", value)
				},
				notBefore: DateTime.UtcNow,
				expires: DateTime.UtcNow.AddSeconds(expiresec),
				signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
			);
			return new JwtSecurityTokenHandler().WriteToken(jwt);
		}
		public static string GenerateRefreshToken(string value, Int32 expiresec = 86400 * 30)
		{
			var key = new SymmetricSecurityKey(Common.GetKey());
			var jwt = new JwtSecurityToken(issuer: "Sakamoto",
				claims: new Claim[] {
					new Claim("token", value)
				},
				notBefore: DateTime.UtcNow,
				signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
			);
			return new JwtSecurityTokenHandler().WriteToken(jwt);
		}

		private static Random random = new Random();
		public static string RandomString(int length = 64)
		{
			const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-=!|";
			return new string(Enumerable.Repeat(chars, length)
			  .Select(s => s[random.Next(s.Length)]).ToArray());
		}

		public static string GetIdFromHttpContext(HttpContext context)
		{
			var identity = context.User.Identity as ClaimsIdentity;
			return GetIdFromClaimsIdentity(identity);
		}
		public static string GetIdFromClaimsIdentity(ClaimsIdentity identity)
		{
			if (identity != null)
			{
				var claims = identity.Claims;
				var id = claims.FirstOrDefault(a => a.Type == "token");
				return id.Value;
			}
			return null;
		}
		
	}
}
