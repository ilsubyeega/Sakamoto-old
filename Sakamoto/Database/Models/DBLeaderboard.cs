using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Database.Models
{
	[Table("leaderboard")]
	public class DBLeaderboard
	{
		[Column("user_id")]
		[Key]
		public int UserId { get; set; }
		[Column("gamemode")]
		[Key]
		public int GameMode { get; set; }
		[Column("type")]
		[Key]
		public int Type { get; set; }
		[Column("country")]
		[Key]
		public string Country { get; set; } // 2 letter country
		[Column("rank")]
		public int Rank { get; set; }
	}
}
