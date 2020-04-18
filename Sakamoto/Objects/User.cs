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

		public bool is_restricted;

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
		public UserGame()
		{

		}
		public UserGame(int pp_osu, int pp_osutaiko, int pp_osucatch, int pp_osumania, 
			int pp_osu_relax, int pp_osutaiko_relax, int pp_osucatch_relax, 
			int playcount_osu, int playcount_osutaiko, int playcount_osucatch, int playcount_osumania, 
			int playcount_osu_relax, int playcount_osutaiko_relax, int playcount_osucatch_relax)
		{
			this.pp_osu = pp_osu;
			this.pp_osutaiko = pp_osutaiko;
			this.pp_osucatch = pp_osucatch;
			this.pp_osumania = pp_osumania;
			this.pp_osu_relax = pp_osu_relax;
			this.pp_osutaiko_relax = pp_osutaiko_relax;
			this.pp_osucatch_relax = pp_osucatch_relax;
			this.playcount_osu = playcount_osu;
			this.playcount_osutaiko = playcount_osutaiko;
			this.playcount_osucatch = playcount_osucatch;
			this.playcount_osumania = playcount_osumania;
			this.playcount_osu_relax = playcount_osu_relax;
			this.playcount_osutaiko_relax = playcount_osutaiko_relax;
			this.playcount_osucatch_relax = playcount_osucatch_relax;
		}


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
