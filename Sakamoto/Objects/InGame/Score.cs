using osu.Framework.Input.Bindings;
using osu.Shared;
using Sakamoto.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sakamoto.Objects.InGame
{
	public class Score
	{
		public int userid;
		public long scoreid;
		public int time;
		public int pp;
		public bool Loved = false;

		public string username;
		public string beatmap_hash;
		public int score = 0;
		public Enums.Mods mods = 0;
		public short count_300 = 0;
		public short count_100 = 0;
		public short count_50 = 0;
		public short count_geki = 0;
		public short count_katu = 0;
		public short count_miss = 0;
		public double accuracy;
		public short MaxCombo = 0;
		public bool isFullCombo = false;
		public string rank;
		public GameMode gamemode;
		public int replayable = 1;
		/// <summary>
		/// To osu! Data
		/// type 0 = score, type 1 = pp
		/// </summary>
		/// <returns>osu! string</returns>

		public string AppendToString(string rank)
		{
			return String.Format("{}|{}|{}|{}|{}|{}|{}|{}|{}|{}|{}|{}|{}|{}|{}|{}\n",
				scoreid,
				username,
				score,
				MaxCombo,
				count_50,
				count_100,
				count_300,
				count_miss,
				count_katu,
				count_geki,
				isFullCombo ? "0": "1",
				mods,
				userid,
				rank,
				time,
				replayable
				);
		}
	}
}
