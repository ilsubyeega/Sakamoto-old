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
				q = q.Where(a => (int)a["LanguageId"] == languageid);

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

			BeatmapSortCriteria sortable = BeatmapSortCriteria.Relevance;
			bool isasc = false;

			if (splittedsort.Length == 2 && (splittedsort[1] == "asc" || splittedsort[1] == "desc")
				&& Enum.TryParse(typeof(BeatmapSortCriteria), splittedsort[0], true, out object? sortobj))
			{
				isasc = splittedsort[1] == "asc"; // true: asc, false: desc
				sortable = (BeatmapSortCriteria)sortobj;
				switch (sortable)
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

			q = q.Skip(offset).Take(limit > 100 ? 50 : limit);
			q = q.Include(a => a.Beatmaps);
			ParseAdvancedQuery(ref q, query);
			
			list = q.ToArray();




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
			var sortstring = isasc ? "asc" : "desc" ;
			var result = new
			{
				beatmapsets = blist,
				cursor = new
				{
					current = offset,
					next = offset + 50,
					limit = 50
				},
				search = new {
					sort = $"{sortable.ToString().ToLower()}_{sortstring}"
				},
				recommended_difficulty = 7,
				total = blist.Count()
			};
			return StatusCode(200, result);
		}
		// returns query that replaced advanced query with blank
		private string ParseAdvancedQuery(ref IQueryable<DBBeatmapSet> q, string query)
		{
			var fixedquery = "";
			var splitted1 = query.Split(" ");
			foreach (var _a in splitted1)
			{
				if (matchable.Any(a => _a.Contains(a)))
				{
					fixedquery = fixedquery.Replace(_a, "");
					
				}
			}

			return fixedquery;
		}
		private IQueryable<DBBeatmapSet> AddAdvancedQuery(IQueryable<DBBeatmapSet> q, (string, string, string, bool, bool) keymatch, object value, string match)
		{
			var key = keymatch.Item1;
			var dbkey = keymatch.Item2;
			var _type = keymatch.Item3;
			var unsigned = keymatch.Item4;
			var fromset = keymatch.Item5;

			if (key == "length")
				value = ParseLength((string)value);

			switch (keymatch.Item3)
			{
				case "float":
					if (unsigned && (float)value < 0) throw new Exception($"Value {key} is negative.");
					switch (_type)
					{
						case "<":
							return fromset ? q.Where(a => (float)a[dbkey] < (float)value) : q.Where(a => a.Beatmaps.Any(b => (float)b[dbkey] < (float)value));

						case "<:":
						case "<=":
							return fromset ? q.Where(a => (float)a[dbkey] <= (float)value) : q.Where(a => a.Beatmaps.Any(b => (float)b[dbkey] <= (float)value));

						case ">":
							return fromset ? q.Where(a => (float)a[dbkey] > (float)value) : q.Where(a => a.Beatmaps.Any(b => (float)b[dbkey] > (float)value));

						case ">:":
						case ">=":
							return fromset ? q.Where(a => (float)a[dbkey] >= (float)value) : q.Where(a => a.Beatmaps.Any(b => (float)b[dbkey] >= (float)value));

						case "=":
						case ":":
							return fromset ? q.Where(a => (float)a[dbkey] == (float)value) : q.Where(a => a.Beatmaps.Any(b => (float)b[dbkey] == (float)value));

						default: throw new Exception("invalid logic");
					}

				case "int":
					if (unsigned && (int)value < 0) throw new Exception($"Value {key} is negative.");
					switch (_type)
					{
						case "<":
							return fromset ? q.Where(a => (int)a[dbkey] < (int)value) : q.Where(a => a.Beatmaps.Any(b => (int)b[dbkey] < (int)value));

						case "<:":
						case "<=":
							return fromset ? q.Where(a => (int)a[dbkey] <= (int)value) : q.Where(a => a.Beatmaps.Any(b => (int)b[dbkey] <= (int)value));

						case ">":
							return fromset ? q.Where(a => (int)a[dbkey] > (int)value) : q.Where(a => a.Beatmaps.Any(b => (int)b[dbkey] > (int)value));

						case ">:":
						case ">=":
							return fromset ? q.Where(a => (int)a[dbkey] >= (int)value) : q.Where(a => a.Beatmaps.Any(b => (int)b[dbkey] >= (int)value));

						case "=":
						case ":":
							return fromset ? q.Where(a => (int)a[dbkey] == (int)value) : q.Where(a => a.Beatmaps.Any(b => (int)b[dbkey] == (int)value));

						default: throw new Exception("invalid logic");
					}

				case "string":
					switch (_type)
					{
						case "=":
						case ":":
							return fromset ? q.Where(a => (string)a[dbkey] == (string)value) : q.Where(a => a.Beatmaps.Any(b => (string)b[dbkey] == (string)value));

						default: throw new Exception("invalid logic");
					}
				default:
					throw new Exception($"{keymatch.Item3} Type isnt supported yet.");
			}
		}
		private static string[] matchable = new string[] { "<", "<=", "<:", ">", ">=", ">:", "=", ":" };


		// key, dbkey, type, cannotbenegative, queryfromset
		private static (string, string, string, bool, bool)[] KeyMatch = new (string, string, string, bool, bool)[]
		{
			("stars", "DiffRating", "float", true, false),
			("ar", "DiffApproach", "float", true, false),
			("dr", "DiffDrain", "float", true, false),
			("hp", "DiffDrain", "float", true, false),
			("cs", "DiffSize", "float", true, false),
			("od", "DiffOverall", "float", true, false),
			("bpm", "BPM", "float", true, false),

			("length", "TotalLength", "int", true, false), // 1000ms -> 1
			("keys", "DiffSize", "int", true, false),
			//("divisor", "", "int", true, false),
			("status", "Ranked", "int", false, false),
			("creator", "Creator", "string", true, true),
			("artist", "Artist", "string", true, true)
		};
		private static int ParseLength(string value)
		{
			var _numstr = String.Join("", value.ToCharArray().Where(a => Char.IsDigit(a)));
			var _type = String.Join("", value.ToCharArray().Where(a => Char.IsLetter(a)));
			if (_numstr + _type != value || int.TryParse(_numstr, out int _num)) return 0;
			switch (_type)
			{
				default:
					return -1;
				case "ms":
					return Convert.ToInt32(_num * 0.001);
				case "s":
					return _num * 1;
				case "m":
					return _num * 60;
				case "h":
					return _num * 3600;
			}
		}
	}
}
