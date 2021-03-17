using Newtonsoft.Json;
using Sakamoto.Api;
using Sakamoto.Database;
using Sakamoto.Database.Models.Beatmap;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Helper
{
	public static class BeatmapSeeder
	{
		private static string BeatmapsetFetchUrl(int id) => $"{OsuApi.API_ROOT}/beatmapsets/{id}";
		private static string BeatmapFetchUrl(int id) => $"{OsuApi.API_ROOT}/beatmaps/{id}";

		private static List<int> QueuedSet = new List<int>();
		public static bool IsExistsSet(int beatmapset_id, MariaDBContext context) => context.BeatmapSets.Any(a => a.BeatmapsetId == beatmapset_id);
		public static bool IsQueuedSet(int beatmapset_id) => QueuedSet.Contains(beatmapset_id);
		public static async Task SeedSetFromBeatmap(int beatmap_id, bool overwrite, MariaDBContext context)
		{
			var value = await OsuApi.FetchBeatmap(beatmap_id);
			if (value == null) return;
			await SeedSet(value.BeatmapSetId, false, context);
		}
		public static async Task SeedSet(int beatmapset_id, bool overwrite, MariaDBContext context)
		{
			if (IsQueuedSet(beatmapset_id)) return;
			if (!overwrite && IsExistsSet(beatmapset_id, context)) return;

			QueuedSet.Add(beatmapset_id);

			Console.WriteLine($"BeatmapSeeder: Seeding {beatmapset_id}....");
			var stopwatch = new Stopwatch();
			stopwatch.Start();
			// Fetch the Beatmap from osu! api v2.
			JsonBeatmapSet value;
			try
			{
				var rs = await OsuApi.TryReqeust(BeatmapsetFetchUrl(beatmapset_id));
				if (rs == null) return;
				value = JsonConvert.DeserializeObject<JsonBeatmapSet>(rs);
			}
			catch (Exception e)
			{

				Console.WriteLine($"BeatmapSeeder: Failed to fetch beatmapid {beatmapset_id}");
				Console.WriteLine(e.Message);
				QueuedSet.Remove(beatmapset_id);
				return;
			}
			try
			{
				var beatmapsets = new DBBeatmapSet
				{
					BeatmapsetId = value.Id,
					Creator = value.Creator,
					Artist = value.Artist,
					ArtistUnicode = value.ArtistUnicode,
					Title = value.Title,
					TitleUnicode = value.TitleUnicode,
					Source = value.Source,
					TagsRaw = value.Tags,
					IsVideo = value.HasVideo,
					IsStoryboard = value.HasStoryBoard,
					//IsEpilepsy
					IsNsfw = value.IsNsfw,
					Bpm = value.BPM,
					VersionsAvailable = value?.Beatmaps == null ? 0 : value.Beatmaps.Count(),
					SubmitDate = ParseTime(value.SubmittedDate).ToUnixTimeSeconds(),
					UpdatedDate = ParseTime(value.LastUpdated).ToUnixTimeSeconds(),
					KeesuUpdatedDate = DateTimeOffset.Now.ToUnixTimeSeconds(),
					//Rating
					GenreId = value.Genre.Id,
					LanguageId = value.Language.Id,
					IsDownloadable = value.DownloadAvaility?.DownloadDisabled ?? true,
					DownloadDisabledUrl = value.DownloadAvaility?.MoreInformation,
					ShouldRefresh = value.Ranked <= 0,
					Ranked = value.Ranked,
				};

				var tasks = new List<Task<DBBeatmap>>();
				foreach (var bc in value.Beatmaps)
					tasks.Add(ToDatabasedBeatmap(bc));

				Task.WaitAll(tasks.ToArray());

				foreach (var task in tasks)
				{
					var dbbtmprs = task.Result;
					if (dbbtmprs == null) throw new Exception($"Beatmap wasnt found. Set id: {beatmapset_id}"); // should be exist
					context.Beatmaps.Add(dbbtmprs);
				}

				context.BeatmapSets.Add(beatmapsets);

				await context.SaveChangesAsync();
				QueuedSet.Remove(beatmapset_id);

				stopwatch.Stop();
				var timeelapsed = stopwatch.ElapsedMilliseconds;

				Console.WriteLine($"BeatmapSeeder: Seeded {beatmapset_id} [Maps:{tasks.Count}] ({timeelapsed}ms)");
			}


			catch (Exception e)
			{
				Console.WriteLine($"BeatmapSeeder: Failed to seeding beatmaps while creating entitys setid: {beatmapset_id}");
				Console.WriteLine(e.Message);
				Console.WriteLine(e.StackTrace);
				QueuedSet.Remove(beatmapset_id);
				return;
			}


		}
		private static async Task<DBBeatmap> ToDatabasedBeatmap(JsonBeatmapCompact bc)
		{
			var rs = await OsuApi.TryReqeust(BeatmapFetchUrl(bc.Id));
			var rslegacy = await OsuApi.FetchLegacyBeatmap(bc.Id);
			if (rs == null) return null; // this should be exist since it has beatmapcompact from beatmapset.
			if (rslegacy == null)
			{
				Console.WriteLine("BeatmapSeeder: Failed to fetch from osu api v1.");
			}
			JsonBeatmap b = JsonConvert.DeserializeObject<JsonBeatmap>(rs);
			var beatmap = new DBBeatmap
			{
				BeatmapsetId = b.BeatmapSetId,
				BeatmapId = b.Id,
				Checksum = rslegacy?.Checksum, // provided by api v1 until api v2 support this https://github.com/ppy/osu-web/issues/6777
				DifficultyName = b.Version,
				TotalLength = b.TotalLength,
				HitLength = b.HitLength,
				Ranked = (int)b.Ranked,
				KeesuRanked = 0,
				DiffRating = b.DifficultyRating,
				DiffSize = b.CircleSize,
				DiffDrain = b.Drain,
				DiffOverall = b.Accuracy,
				DiffApproach = b.AR,
				CountTotal = b.CountCircles + b.CountSliders + b.CountSpinners,
				CountNormal = b.CountCircles,
				CountSlider = b.CountSliders,
				CountSpinner = b.CountSpinners,
				BPM = b.BPM,
				PlayCount = 0,
				PlayMode = (byte)b.Mode,
				UpdatedDate = ParseTime(b.LastUpdated).ToUnixTimeSeconds(),
				MaxCombo = b.MaxCombo
			};
			return beatmap;
		}
		private static DateTimeOffset ParseTime(string time) => DateTimeOffset.Parse(time);
	}
}
