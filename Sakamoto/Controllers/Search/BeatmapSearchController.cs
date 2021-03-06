using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sakamoto.Api;
using Sakamoto.Database;
using Sakamoto.Database.Models.Beatmap;
using Sakamoto.Enums.Beatmap;
using Sakamoto.Transformer;
using Sakamoto.Util.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Controllers.Search
{
	[ApiController]
	[Route("api/v2/")]
	[Authorize]
	public class BeatmapSearchController : ControllerBase
	{
		private readonly MariaDBContext _dbcontext;
		public BeatmapSearchController(MariaDBContext mariaDBContext) { _dbcontext = mariaDBContext; }

		[HttpGet("beatmapsets/search")]
		public async Task<IActionResult> SearchBeatmap(
			[FromQuery(Name = "q")] string query = null, // query
			[FromQuery(Name = "m")] int? rulesetid = null, // query
			[FromQuery(Name = "s")] string searchcategory = null, // category
			[FromQuery(Name = "g")] int genreid = 0, // genre
			[FromQuery(Name = "l")] int languageid = 0, // language
			[FromQuery(Name = "sort")] string sorttext = "relevance_desc", // SortCriteria(lower)_direction
			[FromQuery(Name = "e")] string extras = null, // extra (divided by .)
			[FromQuery(Name = "r")] string ranks = null, // ranks (divided by .)
			[FromQuery(Name = "played")] string has_played = "any", // any, played, unplayed
			[FromQuery(Name = "nsfw")] bool nsfw = true,
			[FromQuery(Name = "cursor[next]")] int offset = 0,
			[FromQuery(Name = "cursor[limit]")] int limit = 50)
		{
			var userid = (int)HttpContext.Items["userId"];
			DBBeatmapSet[] list;

			IQueryable<DBBeatmapSet> q = _dbcontext.BeatmapSets;


			if (searchcategory != null && Enum.TryParse(typeof(BeatmapCategory), searchcategory, true, out object? categoryobj))
			{
				var categoryenum = (BeatmapCategory)categoryobj;
				switch (categoryenum)
				{
					default:
						break;
					case BeatmapCategory.Leaderboard:
						q = q.Where(a => a.Ranked > (int)BeatmapStatus.pending);
						break;
					case BeatmapCategory.Ranked:
						q = q.Where(a => a.Ranked == (int)BeatmapStatus.ranked);
						break;
					case BeatmapCategory.Qualified:
						q = q.Where(a => a.Ranked == (int)BeatmapStatus.qualified);
						break;
					case BeatmapCategory.Loved:
						q = q.Where(a => a.Ranked == (int)BeatmapStatus.loved);
						break;
					case BeatmapCategory.Favourites:
						var favs = _dbcontext.MapFavourites.Where(a => a.UserId == userid);
						var favlist = new List<int>();
						foreach (var favv in favs)
							favlist.Add(favv.BeatmapsetId);
						var favarr = favlist.ToArray();
						q = q.Where(a => favarr.Contains(a.BeatmapsetId));
						break;
					case BeatmapCategory.Pending:
						q = q.Where(a => a.Ranked == (int)BeatmapStatus.pending);
						break;
					case BeatmapCategory.Graveyard:
						q = q.Where(a => a.Ranked == (int)BeatmapStatus.graveyard);
						break;
					case BeatmapCategory.Mine:
						q = q.Where(a => a.User == userid);
						break;
				}
			}

			if (genreid != 0 && Enum.IsDefined(typeof(BeatmapGenre), genreid))
				q = q.Where(a => a.GenreId == genreid);
			if (languageid != 0 && Enum.IsDefined(typeof(BeatmapLanguage), languageid))
				q = q.Where(a => a.LanguageId == languageid);


			q = q.Where(a => a.IsNsfw == nsfw);





			var fulltextquery = FulltextUtil.ToQuery(query);
			if (fulltextquery != null)
			{
				q = q.Where(a => EF.Functions.Match(
					new[] { a.Creator, a.Artist, a.ArtistUnicode, a.Title, a.TitleUnicode, a.Source, a.TagsRaw },
					fulltextquery, MySqlMatchSearchMode.Boolean))
					.OrderByDescending(a => EF.Functions.Match(
						new[] { a.Creator, a.Artist, a.ArtistUnicode, a.Title, a.TitleUnicode, a.Source, a.TagsRaw },
					fulltextquery, MySqlMatchSearchMode.Boolean));
			}

			string[] splittedextra = extras == null ? new string[] { } : extras.Split(".");
			foreach (var splittedvalue in splittedextra)
			{
				if (splittedvalue == "video")
					q = q.Where(a => a.IsVideo == true);
				if (splittedvalue == "storyboard")
					q = q.Where(a => a.IsStoryboard == true);
			}

			var splittedsort = sorttext.Split("_", StringSplitOptions.RemoveEmptyEntries);
			if (splittedsort.Length == 2 && (splittedsort[1] == "asc" || splittedsort[1] == "desc")
				&& Enum.TryParse(typeof(BeatmapSortCriteria), splittedsort[0], true, out object? sortobj))
			{
				var isasc = splittedsort[1] == "asc"; // true: asc, false: desc
				switch ((BeatmapSortCriteria)sortobj)
				{
					default:
					case BeatmapSortCriteria.Relevance:
						if (isasc)
							q = q.OrderBy(a => EF.Functions.Match(
								new[] { a.Creator, a.Artist, a.ArtistUnicode, a.Title, a.TitleUnicode, a.Source, a.TagsRaw },
							fulltextquery, MySqlMatchSearchMode.Boolean));
						else
							q = q.OrderByDescending(a => EF.Functions.Match(
								new[] { a.Creator, a.Artist, a.ArtistUnicode, a.Title, a.TitleUnicode, a.Source, a.TagsRaw },
							fulltextquery, MySqlMatchSearchMode.Boolean));
						break;
					case BeatmapSortCriteria.Title:
						q = isasc ? q.OrderBy(a => a.Title) : q.OrderByDescending(a => a.Title);
						break;
					case BeatmapSortCriteria.Artist:
						q = isasc ? q.OrderBy(a => a.Artist) : q.OrderByDescending(a => a.Artist);
						break;
					case BeatmapSortCriteria.Ranked:
						q = isasc ? q.OrderBy(a => a.UpdatedDate) : q.OrderByDescending(a => a.UpdatedDate);
						break;
					case BeatmapSortCriteria.Plays:
						q = isasc ? q.OrderBy(a => a.PlayCount) : q.OrderByDescending(a => a.PlayCount);
						break;
					case BeatmapSortCriteria.Favourites:
						q = isasc ? q.OrderBy(a => a.FavouriteCount) : q.OrderByDescending(a => a.FavouriteCount);
						break;
				}
			}

			list = q.Skip(offset).Take(limit > 100 ? 50 : limit).ToArray();

			var blist = new List<JsonBeatmapSet>();
			foreach (var a in list)
			{
				var bblist = new List<JsonBeatmapCompact>();
				var b = _dbcontext.Beatmaps.Where(o => a.BeatmapsetId == o.BeatmapsetId).ToArray();
				foreach (var i in b)
					bblist.Add(i.ToJsonBeatmapCompact());
				var k = a.ToJsonBeatmapSet();
				k.Beatmaps = bblist.ToArray();
				blist.Add(k);
			}
			var result = new
			{
				beatmapsets = blist,
				cursor = new
				{
					current = offset,
					next = offset + 50,
					limit = 50
				},
				recommended_difficulty = 7,
				total = blist.Count()
			};
			return StatusCode(200, result);
		}
	}
}
