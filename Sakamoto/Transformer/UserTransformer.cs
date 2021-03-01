using Sakamoto.Api;
using Sakamoto.Database.Models;
using Sakamoto.Enums;
using System;
using System.Linq;

namespace Sakamoto.Transformer.ResponseTransformer
{
	public static class UserTransformer
	{
		public static JsonUserCompact ToUserCompact(this DBUser user)
		{
			var compact = new JsonUserCompact();
			MoveCompact(compact, user);
			return compact;
		}
		private static void MoveCompact(JsonUserCompact jsonuser, DBUser user)
		{
			jsonuser.AvatarUrl = $"https://keesu.ilsubyeega.com/avatar/{user.Id}.png";
			jsonuser.Country = new JsonCountry
			{
				FlagName = "KR",
				Display = null,
				FullName = "South Korea"
			};
			jsonuser.CountryCode = "KR";
			jsonuser.Cover = new JsonCover
			{
				Url = "https://keesu.ilsubyeega.com/static/cover/1.png",
				Id = "1"
			};
			jsonuser.CurrentModeRank = 0;
			jsonuser.DefaultGroup = "default";
			jsonuser.FollowerCount = 69;
			jsonuser.Groups = null;
			jsonuser.Id = user.Id;
			jsonuser.IsActive = user.LastVisit + (60 * 60 * 30) > DateTimeOffset.Now.ToUnixTimeSeconds();
			jsonuser.IsRestricted = false;
			jsonuser.IsSilenced = false;
			jsonuser.IsDeleted = false;
			jsonuser.IsOnline = false;
			jsonuser.IsSupporter = true;
			jsonuser.LastVisit = user.LastVisit.HasValue ? DateTimeOffset.FromUnixTimeSeconds(user.LastVisit.Value).ToString("o") : DateTimeOffset.Now.ToString("o");
			jsonuser.PmFriendsOnly = false;
			jsonuser.ProfileColour = "fff";
			jsonuser.SupporterLevel = 10;
			jsonuser.Username = user.UserName;
			jsonuser.PlayMode = (GameMode)user.PlayMode;
			jsonuser.ProfileColour = user.Color;
		}
		public static JsonUser ToUser(this DBUser user, DBUserStat stat)
		{
			var jsonuser = new JsonUser();
			MoveCompact(jsonuser, user);
			MoveUser(jsonuser, user);
			jsonuser.Statistics = stat.ToUserStatistics();
			return jsonuser;
		}
		private static void MoveUser(JsonUser jsonuser, DBUser user)
		{
			jsonuser.CommentCount = 0;
			jsonuser.CoverUrl = jsonuser.Cover.CustomUrl ?? jsonuser.Cover.Url;
			jsonuser.Discord = user.CustomDiscord;
			jsonuser.HasSupported = user.HasSupported;
			jsonuser.Interests = user.CustomInterest;
			jsonuser.JoinDate = DateTimeOffset.FromUnixTimeSeconds(user.RegisterationDate.Value).ToString("o");
			jsonuser.KudosuInfo = new JsonKudosuInfo
			{
				Total = 0,
				Available = 0
			};
			jsonuser.Location = user.CustomLocations;
			jsonuser.MaxBlocks = 25;
			jsonuser.MaxFriends = 250;
			jsonuser.Occupation = null;
			jsonuser.PlayStyle = new string[] {
				"keyboard"
			};
			jsonuser.PostCount = 0;
			jsonuser.ProfileOrder = new string[]
			{
					"top_ranks",
					"historical",
					"recent_activity",
					"medals",
					"beatmaps",
					"kudosu",
					"me"
			};
			jsonuser.Skype = user.CustomSkype;
			jsonuser.Twitter = user.CustomTwitter;
			jsonuser.Website = user.CustomWebsite;
			jsonuser.RankHistory = new JsonRankHistory
			{
				Mode = "osu",
				Data = Enumerable.Repeat(1, 3).ToArray()
			};

		}
		public static JsonUserStatistics ToUserStatistics(this DBUserStat stat)
		{
			var a = new JsonUserStatistics();
			MoveStatics(a, stat);
			return a;
		}
		private static void MoveStatics(JsonUserStatistics stat, DBUserStat db)
		{
			stat.Level = new JsonLevel
			{
				Current = 1,
				Progress = 0
			};
			stat.GlobalRank = 1;
			stat.PP = 6974;
			stat.PPRank = 1;
			stat.RankedScore = 19721121;
			stat.HitAccuracy = 69.74;
			stat.PlayCount = 2021;
			stat.PlayTime = 60 * 60 * 24 * 3 + 3600;
			stat.TotalScore = 19721121;
			stat.TotalHits = 1972;
			stat.MaximumCombo = 1972;
			stat.ReaplysWatchedByOthers = 0;
			stat.IsRanked = true;
			stat.Grades = new JsonGradeCounts
			{
				SSH = 0,
				SS = 0,
				SH = 0,
				S = 0,
				A = 0
			};
		}
	}
}
