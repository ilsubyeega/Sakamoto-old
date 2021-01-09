using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Database.Models
{
	[Table("user_stats")]
	public class DBUserStat
	{
		[Key]
		[Column("user_id")]
		public int UserId { get; set; }
		[Column("ruleset_id")]
		public byte RuleSetID { get; set; }
		[Column("count300")]
		public long Count300 { get; set; }
		[Column("count100")]
		public long Count100 { get; set; }
		[Column("count50")]
		public long Count50 { get; set; }
		[Column("count_miss")]
		public long CountMiss { get; set; }
		[Column("accuracy")]
		public decimal Accuracy { get; set; }
		[Column("playcount")]
		public int PlayCount {	get; set; }
		[Column("ranked_score")]
		public long RankedScore { get; set; }
		[Column("total_score")]
		public long TotalScore { get; set; }
		[Column("total_hitcount")]
		public int TotalHit { get; set; }
		[Column("xh_rank_count")]
		public int XHCount { get; set; }
		[Column("x_rank_count")]
		public int SSCount { get; set; }
		[Column("sh_rank_count")]
		public int SHCount { get; set; }
		[Column("s_rank_count")]
		public int SCount { get; set; }
		[Column("a_rank_count")]
		public int ACount { get; set; }
		[Column("b_rank_count")]
		public int BCount { get; set; }
		[Column("c_rank_count")]
		public int CCount { get; set; }
		[Column("d_rank_count")]
		public int DCount { get; set; }
		[Column("performance")]
		public decimal Performance { get; set; }
		[Column("rank")]
		public int GlobalRank { get; set; }
		[Column("country_rank")]
		public int CountryRank { get; set; }
		[Column("replay_popularity")]
		public int ReplayCount { get; set; }
		[Column("fail_count")]
		public int FailCount { get; set; }
		[Column("pass_count")]
		public int PassCount { get; set; }
		[Column("max_combo")]
		public int MaxCombo { get; set; }
		[Column("last_played")]
		public long LastPlayed { get; set; }
		[Column("total_sconds_played")]
		public int TotalPlayed { get; set; }
		[Column("score_best_count")]
		public int ScoreBestCount { get; set; }
	}
}
