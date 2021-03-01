using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Database.Models.Beatmap
{
	[Table("beatmapset_favourites")]
	public class DBBeatmapsetFavourite
	{
		[Key, Column("user_id", Order = 1)]
		public int UserId;
		[Key, Column("beatmapset_id", Order = 2)]
		public int BeatmapsetId;
		[Column("favourited_at")]
		public long FavouritedAt;

	}
}
