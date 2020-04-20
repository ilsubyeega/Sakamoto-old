using HOPEless.Bancho;
using HOPEless.Bancho.Objects;
using osu.Shared;
using Sakamoto.Enums;
using Sakamoto.Manager;
using System.Collections.Generic;
namespace Sakamoto.Objects
{
	/// <summary>
	/// This is User Class. It should be in <see cref="UserCache"/>.
	/// </summary>
	public class User
	{
		public int userid;
		public string username;
		public string username_safe;

		public string email;
		public string password_md5;
		public string notes;

		public int timezone = 0;
		public byte countryid = 0;

		public bool silenced = false;
		public int silence_end;
		public string silence_reason;

		public PlayerType type = PlayerType.Player;
		public int privileges;
		public bool is_donor;
		public int donor_expire;

		public bool is_restricted;
		public GameType gametype = GameType.Osu;

		public BanchoUserStatus status = new BanchoUserStatus();
		public UserGame pp = new UserGame();

		public Channel[] JoinedChannel()
		{
			return ChatManager.GetListJoinedChannel(userid);
		}
		public void JoinChannel(string name) => ChatManager.joinChannel(name, userid);
		public void LeaveChannel(string name) => ChatManager.LeaveChannel(name, userid);

		public List<int> friends = new List<int>();
		public bool block_nonfriend;

		public List<BanchoPacket> queue = new List<BanchoPacket>();

		public string chotoken;

		public long lasttimestamp = 0;

		public void addQueue(BanchoPacket input)
		{
			queue.Add(input);
		}
		public void ClearQueue()
		{
			queue.Clear();
		}

		public BanchoUserPresence ToPresence()
		{
			return new BanchoUserPresence()
			{
				UserId = this.userid,
				UsesOsuClient = true,
				Username = this.username,
				Timezone = this.timezone,
				CountryCode = countryid,
				Longitude = 1.2f,
				Latitude = 1.2f,
				Permissions = PlayerRank.Supporter,
				Rank = 1,
				PlayMode = (byte)GameTypeUtil.getShared(gametype)
			};
		}
		public BanchoUserData ToUserData()
		{
			return new BanchoUserData()
			{
				UserId = this.userid,
				Status = status,
				RankedScore = 1000,
				Accuracy = pp.GetAccByGame(gametype),
				Playcount = pp.GetPlayCountByGame(gametype),
				TotalScore = 1000,
				Rank = 1,
				Performance = pp.GetPerformanceByGame(gametype)
			};
		}
	}

	public class UserGame
	{
		/// <summary>
		/// This is full list of total modes of osu!
		/// 0~3 = osu!, osu!taiko, osu!catch, osu!mania
		/// 4~6 = osu! (relax), osu!taiko (relax), osu!catch (relax)
		/// 7 = osu! (autopilot)
		/// 4 + 3 + 1 - 1 = 7
		/// </summary>
		static readonly int totalgame = 7;

		short[] pps = new short[7];
		float[] acc = new float[7];
		int[] playcount = new int[7];
		long[] ranked_score = new long[7];
		long[] total_score = new long[7];
		/// <summary>
		/// Create UserGame instance which status is nothing.
		/// </summary>
		public UserGame()
		{
			pps = new short[7] { 0, 0, 0, 0, 0, 0, 0 };
			acc = new float[7] { 0, 0, 0, 0, 0, 0, 0 };
			playcount = new int[7] { 0, 0, 0, 0, 0, 0, 0 };
			ranked_score = new long[7] { 0, 0, 0, 0, 0, 0, 0 };
			total_score = new long[7] { 0, 0, 0, 0, 0, 0, 0 };

		}
		/// <summary>
		/// Create UserGame instance with some status.
		/// </summary>
		/// <param name="pp_arg">Short arrays of pp</param>
		/// <param name="acc_arg">Float arrays of accuracy</param>
		/// <param name="playcount_arg">Int32 arrays of play count</param>
		/// <param name="rankedscore_arg">Long arrays of ranked socre</param>
		/// <param name="totalscore_arg">Long arrays of total score</param>
		public UserGame(short[] pp_arg, float[] acc_arg, int[] playcount_arg, long[] rankedscore_arg, long[] totalscore_arg)
		{
			pps = pp_arg;
			playcount = playcount_arg;
			ranked_score = rankedscore_arg;
			total_score = totalscore_arg;
		}

		
		
		/// <summary>
		/// Get pp by GameType
		/// </summary>
		/// <param name="type">Game Type</param>
		/// <returns>pp</returns>
		public short GetPerformanceByGame(GameType type)
		{
			if ((int)type > totalgame)
				return 0;
			return pps[(int)type];
		}
		/// <summary>
		/// Get accuracy by GameType
		/// </summary>
		/// <param name="type">Game Type</param>
		/// <returns>Accuracy</returns>
		public float GetAccByGame(GameType type)
		{
			if ((int)type > totalgame)
				return 0;
			return acc[(int)type];
		}
		/// <summary>
		/// Get play count by GameType
		/// </summary>
		/// <param name="type">Game Type</param>
		/// <returns>Play count</returns>
		public int GetPlayCountByGame(GameType type)
		{
			if ((int)type > totalgame)
				return 0;
			return playcount[(int)type];
		}
		/// <summary>
		/// Get ranked socres by GameType
		/// </summary>
		/// <param name="type">Game Type</param>
		/// <returns>Ranked Scores</returns>
		public long GetRankedScoreByGame(GameType type)
		{
			if ((int)type > totalgame)
				return 0;
			return ranked_score[(int)type];
		}
		/// <summary>
		/// Get total scores by GameType
		/// </summary>
		/// <param name="type">Game Type</param>
		/// <returns>Total Scores</returns>
		public long GetTotalScoreByGame(GameType type)
		{
			if ((int)type > totalgame)
				return 0;
			return total_score[(int)type];
		}
	}
}
