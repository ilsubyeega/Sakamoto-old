using Newtonsoft.Json;
using Sakamoto.Enums.Beatmap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Api
{
	public class JsonBeatmapSetCompact
	{
		[JsonProperty("artist")]
		public string Artist;
		[JsonProperty("artist_unicode")] // nullable
		public string ArtistUnicode;
		[JsonProperty("covers")]
		public JsonBeatmapCovers Covers;
		[JsonProperty("favourite_count")]
		public int FavouriteCount;
		[JsonProperty("hype")] // nullable
		public JsonHype Hype;
		[JsonProperty("id")]
		public int Id;
		[JsonProperty("nsfw")]
		public bool IsNsfw;
		[JsonProperty("play_count")]
		public long PlayCount;
		[JsonProperty("preview_url")]
		public string PreviewUrl;
		public void GeneratePreviewUrl() => PreviewUrl = $"https://b.ppy.sh/preview/{Id}.mp3";
		[JsonProperty("source")]
		public string Source;
		[JsonProperty("status")]
		public BeatmapStatus Status;
		[JsonProperty("title")]
		public string Title;
		[JsonProperty("title_unicode")]
		public string TitleUnicode;
		[JsonProperty("creator")]
		public string Creator;
		[JsonProperty("user_id")]
		public int UserId;
		[JsonProperty("video")]
		public bool HasVideo;

		[JsonProperty("beatmaps", NullValueHandling = NullValueHandling.Ignore)]
		public JsonBeatmapCompact[] Beatmaps = null;

		[JsonProperty("converts", NullValueHandling = NullValueHandling.Ignore)]
		public JsonBeatmapCompact[] Converts = null; // should ignore (OR NOT...?)

		[JsonProperty("current_user_attributes", NullValueHandling = NullValueHandling.Ignore)]
		public JsonCurrentUserAttributes CurrentUserAttributes = null;
		[JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
		public JsonDescription Description = null;
		[JsonProperty("discussions", NullValueHandling = NullValueHandling.Ignore)]
		public object Discussions = null;
		[JsonProperty("events", NullValueHandling = NullValueHandling.Ignore)]
		public object Events = null;
		[JsonProperty("genre", NullValueHandling = NullValueHandling.Ignore)]
		public JsonIdName Genre = null;
		[JsonProperty("has_favourited", NullValueHandling = NullValueHandling.Ignore)]
		public bool? HasFavourited = null;
		[JsonProperty("language", NullValueHandling = NullValueHandling.Ignore)]
		public JsonIdName Language = null;
		[JsonProperty("nominations", NullValueHandling = NullValueHandling.Ignore)]
		public object Nominations = null;
		[JsonProperty("ratings", NullValueHandling = NullValueHandling.Ignore)]
		public int[] Ratings = null;
		[JsonProperty("recent_favourites", NullValueHandling = NullValueHandling.Ignore)]
		public JsonUserCompact[] RecentFavourites = new JsonUserCompact[] { };
		[JsonProperty("related_users", NullValueHandling = NullValueHandling.Ignore)]
		public object RelatedUsers = null;
		[JsonProperty("user", NullValueHandling = NullValueHandling.Ignore)]
		public JsonUserCompact User; // Known as beatmap creator.
	}
	// has favoured required for this beatmapset extended
	public class JsonBeatmapSet : JsonBeatmapSetCompact
	{
		[JsonProperty("availability")]
		public JsonDownloadAvaility DownloadAvaility;
		[JsonProperty("bpm")]
		public float BPM;
		[JsonProperty("can_be_hyped")]
		public bool CanBeHyped = false;
		[JsonProperty("discussion_enabled")]
		public bool DiscussionEnabled = false;
		[JsonProperty("discussion_locked")]
		public bool DiscussionLocked = false;
		[JsonProperty("is_scoreable")]
		public bool IsScoreable;
		[JsonProperty("last_updated")]
		public string LastUpdated;
		[JsonProperty("legacy_thread_url")]
		public string LegacyThreadUrl = null; // ignore
		[JsonProperty("nominations_summary")]
		public JsonNominationSummaryMeta NominationSummary;
		[JsonProperty("ranked")]
		public int Ranked;
		[JsonProperty("ranked_date")]
		public string RankedDate;
		[JsonProperty("storyboard")]
		public bool HasStoryBoard;
		[JsonProperty("submitted_date")]
		public string SubmittedDate;
		[JsonProperty("tags")]
		public string Tags;

	}
}
