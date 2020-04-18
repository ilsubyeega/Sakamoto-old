using HOPEless.Bancho;
using HOPEless.Bancho.Objects;
using Sakamoto.Enums;
using System.Collections.Generic;

namespace Sakamoto.Objects
{
	public class User
	{
		public int userid;
		public string username;
		public string username_safe;

		public string email;
		public string password_md5;
		public string notes;

		public bool silenced;
		public int silence_end;
		public string silence_reason;

		public int privileges;
		public bool is_donor;
		public int donor_expire;

		public BanchoUserStatus status = new BanchoUserStatus();
		public UserGame pp = new UserGame();

		public string[] joinedchannel;
		public bool block_nonfriend;

		List<BanchoPacket> queue = new List<BanchoPacket>();

		public void addQueue(BanchoPacket input)
		{
			queue.Add(input);
		}
		public void ClearQueue()
		{
			queue.Clear();
		}
	}

	public class UserGame
	{
		public int pp_osu;
		public int pp_osutaiko;
		public int pp_osucatch;
		public int pp_osumania;
		public int pp_osu_relax;
		public int pp_osutaiko_relax;
		public int pp_osucatch_relax;

		public int playcount_osu;
		public int playcount_osutaiko;
		public int playcount_osucatch;
		public int playcount_osumania;
		public int playcount_osu_relax;
		public int playcount_osutaiko_relax;
		public int playcount_osucatch_relax;

		public int GetPerformanceByGame(GameType type)
		{
			switch (type)
			{
				case GameType.Osu:
					return pp_osu;
				case GameType.OsuTaiko:
					return pp_osutaiko;
				case GameType.OsuCatch:
					return pp_osucatch;
				case GameType.OsuMania:
					return pp_osumania;
				case GameType.OsuRelax:
					return pp_osu_relax;
				case GameType.OsuTaikoRelax:
					return pp_osutaiko_relax;
				case GameType.OsuCatchRelax:
					return pp_osucatch_relax;
				default:
					return 0;
			}
		}
		public int GetPlayCountByGame(GameType type)
		{
			switch (type)
			{
				case GameType.Osu:
					return playcount_osu;
				case GameType.OsuTaiko:
					return playcount_osutaiko;
				case GameType.OsuCatch:
					return playcount_osucatch;
				case GameType.OsuMania:
					return playcount_osumania;
				case GameType.OsuRelax:
					return playcount_osu_relax;
				case GameType.OsuTaikoRelax:
					return playcount_osutaiko_relax;
				case GameType.OsuCatchRelax:
					return playcount_osucatch_relax;
				default:
					return 0;
			}
		}
	}
}
