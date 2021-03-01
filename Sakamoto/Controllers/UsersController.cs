using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sakamoto.Api;
using Sakamoto.Database;
using Sakamoto.Enums;
using Sakamoto.Enums.Beatmap;
using Sakamoto.Transformer;
using Sakamoto.Transformer.ResponseTransformer;
using Sakamoto.Util;
using Sakamoto.Util.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Controllers
{
	[Route("api/v2/")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly MariaDBContext _dbcontext;
		public UsersController(MariaDBContext mariaDBContext) { _dbcontext = mariaDBContext; }
		private object Error(string value) => new
		{
			error = value
		};
		/// <summary>
		/// Similar to Get User but with authenticated user (token owner) as user id.
		/// </summary>
		/// <param name="mode">GameMode. User default mode will be used if not specified.</param>

		[HttpGet("me/{mode?}")]
		[Authorize]
		public async Task<IActionResult> Me(string mode = "osu")
		{
			return await UserQuery((int)HttpContext.Items["userId"], (GameMode?)Enum.Parse(typeof(GameMode), mode) ?? GameMode.osu);
		}
		/// <summary>
		/// Returns list of users.
		/// </summary>
		/// <param name="ids">User id to be returned. Specify once for each user id requested. Up to 50 users can be requested at once.</param>
		[HttpGet("users/"), Produces("text/plain")]
		[Authorize]
		public async Task<IActionResult> UserArray([FromQuery(Name = "ids[]")] int[] ids)
		{
			if (ids.Length == 0) return StatusCode(401, Error("Bad request. (No array)"));
			var list = new List<int>(ids);
			var user = await _dbcontext.Users.Where(a => list.Contains(a.Id)).ToArrayAsync();
			var userstats = await _dbcontext.UserStats.Where(a => list.Contains(a.UserId)).ToArrayAsync();

			var rs = new List<JsonUser>();
			foreach (var o in user)
			{
				var us = userstats.FirstOrDefault(a => a.UserId == o.Id);
				if (us == null) continue;
				rs.Add(o.ToUser(us));
			}
			return StatusCode(200, rs.SerializeObject());
		}


		[HttpGet("users/{userid}")]
		[Authorize]
		public async Task<IActionResult> Users(int userid) => await UserQuery(userid, null);
		[HttpGet("users/{userid}/osu")]
		[Authorize]
		public async Task<IActionResult> UsersOsu(int userid) => await UserQuery(userid, GameMode.osu);
		[HttpGet("users/{userid}/taiko")]
		[Authorize]
		public async Task<IActionResult> UsersTaiko(int userid) => await UserQuery(userid, GameMode.taiko);
		[HttpGet("users/{userid}/fruits")]
		[Authorize]
		public async Task<IActionResult> UsersFruits(int userid) => await UserQuery(userid, GameMode.fruits);
		[HttpGet("users/{userid}/mania")]
		[Authorize]
		public async Task<IActionResult> UsersMania(int userid) => await UserQuery(userid, GameMode.mania);


		/// <summary>
		/// This endpoint returns the detail of specified user.
		/// </summary>
		/// <param name="userid">id of the user.</param>
		/// <param name="gameMode">Gmaemode. User default mode will be used if not specified.</param>
		private async Task<IActionResult> UserQuery(int userid, GameMode? gameMode)
		{
			var user = await _dbcontext.Users.FirstOrDefaultAsync(a => a.Id == userid);
			if (user == null) return StatusCode(404, Error("User not found."));
			var userstats = await _dbcontext.UserStats.FirstOrDefaultAsync(a => a.UserId == user.Id && a.RuleSetID == 0);

			var gameuser = user.ToUser(userstats);

			gameuser.FavouriteBeatmapsetCount = _dbcontext.MapFavourites.Count(a => a.UserId == userid);
			
			var submits = _dbcontext.BeatmapSets.Where(a => a.User == userid);
			if (submits != null)
			{
				gameuser.LovedBeatmapsetCount = submits.Count(a => a.Ranked == (int)BeatmapStatus.loved);
				gameuser.RankedApprovedMapsetCount = submits.Count(a => a.Ranked == (int)BeatmapStatus.ranked || a.Ranked == (int)BeatmapStatus.approved);
				gameuser.PendingMapsetCount = submits.Count(a => a.Ranked == (int)BeatmapStatus.pending);
				gameuser.GraveyardMapsetCount = submits.Count(a => a.Ranked == (int)BeatmapStatus.graveyard);
			}
			

			return StatusCode(200, gameuser);
		}

		/// <summary>
		/// Returns kudosu history.
		/// </summary>
		/// <param name="userid">Id of the user.</param>
		/// <param name="limit">Maximum number of results.</param>
		/// <param name="offset">Result offset for pagination.</param>
		/// <returns></returns>
		[HttpGet("users/{userid}/kudosu")]
		[Authorize]
		public async Task<IActionResult> Kudosu(int userid, int limit = 5, int offset = 0)
		{
			return StatusCode(200, new List<JsonKudosuInfo>().ToArray());
		}
		/// <summary>
		/// This endpoint returns the scores of specified user.
		/// </summary>
		/// <param name="userid">Id of the user.</param>
		/// <param name="type">Score type. Must be one of these: best, firsts, recent.</param>
		/// <param name="include_fails">Only for recent scores, include scores of failed plays. Set to 1 to include them. Defaults to 0.</param>
		/// <param name="mode">GameMode of the scores to be returned. Defaults to the specified user's mode.</param>
		/// <param name="limit">Maximum number of results.</param>
		/// <param name="offset">Result offset for pagination.</param>
		/// <returns></returns>
		[HttpGet("users/{userid}/scores/{type}")]
		public async Task<IActionResult> GetScore(int userid, string type, bool include_fails = false, string mode = "osu", int limit = 20, int offset = 0)
		{
			return StatusCode(200, new List<string>().ToArray()); // todo
		}
		/// <summary>
		/// Returns the beatmaps of specified user.
		/// </summary>
		/// <param name="userid">Id of the user.</param>
		/// <param name="type">Beatmap type.</param>
		/// <param name="limit">Maximum number of results.</param>
		/// <param name="offset">Result offset for pagination.</param>
		/// <returns></returns>
		[HttpGet("users/{userid}/beatmapsets/{type}")]
		public async Task<IActionResult> GetBeatmapSets(int userid, string type, int limit = 10, int offset = 0)
		{
			if (type != "favourite")
				return StatusCode(200, new List<string>().ToArray()); // todo

			var maps = _dbcontext.MapFavourites.Where(a => a.UserId == userid)
				.OrderByDescending(b=> b.FavouritedAt)
				.Skip(offset)
				.Take(limit);
			var list = new List<int>();
			foreach (var a in maps)
				list.Add(a.BeatmapsetId);
			var blist = BeatmapJsonUtil.BeatmapSetWithIdArray(list.ToArray(), true, _dbcontext);
			return StatusCode(200, blist);
		}


		/// <summary>
		/// Returns recent activity.
		/// </summary>
		/// <param name="userid">Id of the user.</param>
		/// <param name="limit">Maximum number of results.</param>
		/// <param name="offset">Result offset for pagination.</param>
		/// <returns></returns>
		[HttpGet("users/{userid}/recent_activity")]
		public async Task<IActionResult> GetRecentActivity(int userid, int limit = 10, int offset = 0)
		{
			return StatusCode(200, new List<string>().ToArray()); // todo
		}





		// undocumented
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
