using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Database.Models.Beatmap
{
	[Table("beatmaps")]
	public class DBBeatmap : DBModel
	{
		[Column("beatmapset_id")]
		public int BeatmapsetId { get; set; }
		[Column("beatmap_id")]
		[Key]
		public int BeatmapId { get; set; }
		[Column("checksum")]
		public string Checksum { get; set; }
		[Column("name")]
		public string DifficultyName { get; set; }
		[Column("total_length")]
		public int TotalLength { get; set; }
		[Column("hit_length")]
		public int HitLength { get; set; }
		[Column("ranked")]
		public int Ranked { get; set; }
		[Column("keesu_ranked")]
		public int? KeesuRanked { get; set; }
		[Column("diff_rating")]
		public float DiffRating { get; set; }
		[Column("diff_size")]
		public float DiffSize { get; set; }
		[Column("diff_drain")]
		public float DiffDrain { get; set; }
		[Column("diff_overall")]
		public float DiffOverall { get; set; } // Known as Accuracy
		[Column("diff_approach")]
		public float DiffApproach { get; set; }
		[Column("countTotal")]
		public int CountTotal { get; set; }
		[Column("countNormal")]
		public int CountNormal { get; set; }
		[Column("countSlider")]
		public int CountSlider { get; set; }
		[Column("countSpinner")]
		public int CountSpinner { get; set; }
		[Column("max_combo")]
		public int? MaxCombo { get; set; }
		[Column("bpm")]
		public float BPM { get; set; }
		[Column("playcount")]
		public int PlayCount { get; set; }
		[Column("playmode")]
		public Byte PlayMode { get; set; }
		[Column("updated_date")]
		public long UpdatedDate { get; set; }
		public virtual DBBeatmapSet BeatmapSet { get; set; }
		public void CopyTo(DBBeatmap to)
		{
			to.BeatmapsetId = BeatmapsetId;
			to.BeatmapId = BeatmapId;
			to.Checksum = Checksum;
			to.DifficultyName = DifficultyName;
			to.TotalLength = TotalLength;
			to.HitLength = HitLength;
			to.Ranked = Ranked;
			to.KeesuRanked = KeesuRanked;
			to.DiffRating = DiffRating;
			to.DiffSize = DiffSize;
			to.DiffDrain = DiffDrain;
			to.DiffOverall = DiffOverall;
			to.DiffApproach = DiffApproach;
			to.CountTotal = CountTotal;
			to.CountNormal = CountNormal;
			to.CountSlider = CountSlider;
			to.CountSpinner = CountSpinner;
			to.MaxCombo = MaxCombo;
			to.BPM = BPM;
			to.PlayCount = PlayCount;
			to.PlayMode = PlayMode;
			to.UpdatedDate = UpdatedDate;
		}
	}
}
