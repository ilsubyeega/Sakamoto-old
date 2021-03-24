using Sakamoto.Api;
using Sakamoto.Database.Models;
using Sakamoto.Database.Models.Beatmap;
using Sakamoto.Enums.Beatmap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Transformer
{
	public static class BeatmapsetTransformer
	{
		public static JsonBeatmapSetCompact ToJsonBeatmapSetCompact(this DBBeatmapSet b)
		{
			var a = new JsonBeatmapSetCompact();
			MoveCompact(a, b);
			return a;
		}
		private static void MoveCompact(JsonBeatmapSetCompact a, DBBeatmapSet b)
		{
			a.Artist = b.Artist;
			a.ArtistUnicode = b.ArtistUnicode;
			//a.Beatmaps
			//a.Converts
			a.Covers = new JsonBeatmapCovers().SetValue(b.BeatmapsetId);
			a.FavouriteCount = b.FavouriteCount;
			a.Hype = new JsonHype();
			a.Id = b.BeatmapsetId;
			a.IsNsfw = b.IsNsfw;
			a.PlayCount = b.PlayCount;
			a.GeneratePreviewUrl();
			a.Source = b.Source;
			a.Status = (BeatmapStatus)b.Ranked;
			a.Title = b.Title;
			a.TitleUnicode = b.TitleUnicode;
			a.HasVideo = b.IsVideo;
			a.Ranked = b.Ranked;
			a.Creator = b.Creator;
			a.UserId = b.User ?? -1;
		}
		public static void IncludeRatings(this JsonBeatmapSetCompact a)
			=> a.Ratings = new int[] { 0, 0, 0, 0, 0 };
		public static JsonBeatmapSet ToJsonBeatmapSet(this DBBeatmapSet b)
		{
			var a = new JsonBeatmapSet();
			MoveCompact(a, b);
			Move(a, b);
			return a;
		}
		public static void Move(JsonBeatmapSet a, DBBeatmapSet b)
		{
			a.DownloadAvaility = new JsonDownloadAvaility
			{
				DownloadDisabled = !b.IsDownloadable,
				MoreInformation = b.DownloadDisabledUrl
			};
			a.BPM = b.Bpm;
			a.IsScoreable = b.Ranked > 0;
			a.LastUpdated = DateTimeOffset.FromUnixTimeSeconds(b.KeesuUpdatedDate).ToString("o");
			a.NominationSummary = new JsonNominationSummaryMeta();
			a.RankedDate = b.KeesuModified ? DateTimeOffset.FromUnixTimeSeconds(b.KeesuUpdatedDate).ToString("o") : DateTimeOffset.FromUnixTimeSeconds(b.UpdatedDate).ToString("o");
			a.HasStoryBoard = b.IsStoryboard;
			a.SubmittedDate = DateTimeOffset.FromUnixTimeSeconds(b.SubmitDate).ToString("o");
			a.Tags = b.TagsRaw;
			a.Creator = b.Creator;
		}
	}
}
