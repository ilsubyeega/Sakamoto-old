using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Sakamoto.Database;
using Sakamoto.Transformer.ResponseTransformer;
using Sakamoto.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Controllers
{
	[Route("api/v2/")]
	[ApiController]
	public class MeController : ControllerBase
	{
		private readonly MariaDBContext _dbcontext;
		public MeController(MariaDBContext mariaDBContext) { _dbcontext = mariaDBContext; }

		[HttpGet("me/{id?}"), Produces("text/plain")]
		[Authorize]
		public async Task<IActionResult> Me(int? id)
		{
			if (id == null) id = 0;

			var token = _dbcontext.AccessTokens.FirstOrDefault(a => a.Id == JwtUtil.GetIdFromHttpContext(HttpContext));
			var user = _dbcontext.Users.FirstOrDefault(a => a.Id == token.UserId);
			var userstats = _dbcontext.UserStats.FirstOrDefault(a => a.UserId == token.UserId && a.RuleSetID == user.PlayMode);
			var gameuser = user.ToGameUser(userstats);
			var json = gameuser.SerializeUser();
			Console.WriteLine(json);
			return StatusCode(200, json);
		}
		[HttpGet("friends"), Produces("text/plain")]
		[Authorize]
		public async Task<IActionResult> Friends()
		{
			/*var token = _dbcontext.AccessTokens.FirstOrDefault(a => a.Id == JwtUtil.GetIdFromHttpContext(HttpContext));
			var user = _dbcontext.Users.FirstOrDefault(a => a.Id == token.UserId);
			var gameuser = user.ToGameUser(userstats);
			var json = gameuser.SerializeObject();
			Console.WriteLine(json);*/

			var list = new List<osu.Game.Users.User>();
			return Ok(list.SerializeObject());
		}


	}
}
