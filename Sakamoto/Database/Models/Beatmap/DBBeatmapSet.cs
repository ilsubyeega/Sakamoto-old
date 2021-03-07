using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Database.Models.Beatmap
{
	[Table("beatmapsets")]
	public class DBBeatmapSet
	{
		[Column("beatmapset_id")]
		[Key]
		public int BeatmapsetId { get; set; }
		[Column("creator")]
		public string Creator { get; set; }
		[Column("artist")]
		public string Artist { get; set; }
		[Column("artist_unicode")]
		public string ArtistUnicode { get; set; }
		[Column("title")]
		public string Title { get; set; }
		[Column("title_unicode")]
		public string TitleUnicode { get; set; }
		[Column("source")]
		public string Source { get; set; }
		[Column("tags")]
		public string TagsRaw { get; set; }
		[Column("is_video")]
		public bool IsVideo { get; set; }
		[Column("is_storyboard")]
		public bool IsStoryboard { get; set; }
		[Column("is_epilepsy")]
		public bool IsEpilepsy { get; set; }
		[Column("is_nsfw")]
		public bool IsNsfw { get; set; }
		[Column("bpm")]
		public float Bpm { get; set; }
		[Column("versions_available")]
		public int VersionsAvailable { get; set; }
		[Column("ranked")]
		public int Ranked { get; set; }
		[Column("keesu_modified")]
		public bool KeesuModified { get; set; }
		[Column("submit_date")]
		public long SubmitDate { get; set; }
		[Column("updated_date")]
		public long UpdatedDate { get; set; }
		[Column("keesu_updated_date")]
		public long KeesuUpdatedDate { get; set; }
		[Column("rating")]
		public float Rating { get; set; }
		[Column("genre_id")]
		public int? GenreId { get; set; }
		[Column("language_id")]
		public int? LanguageId { get; set; }
		[Column("is_downloadable")]
		public bool IsDownloadable { get; set; }
		[Column("download_disabled_url")]
		public string DownloadDisabledUrl { get; set; }
		[Column("favourite_count")]
		public int FavouriteCount { get; set; }
		[Column("playcount")]
		public int PlayCount { get; set; }
		[Column("deleted_at")]
		public int DeletedAt { get; set; }
		[Column("hype")]
		public int Hype { get; set; }
		[Column("should_refresh")]
		public bool ShouldRefresh { get; set; }
		[Column("user")]
		public int? User { get; set; }
		public virtual List<DBBeatmap> Beatmaps { get; set; }
	}
}
