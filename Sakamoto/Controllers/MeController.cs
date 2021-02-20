using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

			var token = await _dbcontext.AccessTokens.FirstOrDefaultAsync(a => a.Id == JwtUtil.GetIdFromHttpContext(HttpContext));
			var user = await _dbcontext.Users.FirstOrDefaultAsync(a => a.Id == token.UserId);
			var userstats = await _dbcontext.UserStats.FirstOrDefaultAsync(a => a.UserId == token.UserId && a.RuleSetID == user.PlayMode);
			var gameuser = user.ToUser(userstats);
			var json = gameuser.SerializeObject();
			Console.WriteLine(json);
			return StatusCode(200, json);
		}
		[HttpGet("users/{userid?}/{modes?}"), Produces("text/plain")]
		[Authorize]
		public async Task<IActionResult> Users(int? userid, string modes = "osu")
		{
			var user = await _dbcontext.Users.FirstOrDefaultAsync(a => a.Id == userid);
			var userstats = await _dbcontext.UserStats.FirstOrDefaultAsync(a => a.UserId == user.Id && a.RuleSetID == 0);
			var gameuser = user.ToUser(userstats);
			var json = gameuser.SerializeObject();
			Console.WriteLine(json);
			return StatusCode(200, json);
		}
		[HttpGet("friends"), Produces("text/plain")]
		[Authorize]
		public IActionResult Friends()
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
