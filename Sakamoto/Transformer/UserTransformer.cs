using osu.Game.Online.API.Requests.Responses;
using Sakamoto.Database.Models;
using Sakamoto.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Transformer.ResponseTransformer
{
	public static class UserTransformer
	{
		public static osu.Game.Users.User ToGameUser(this DBUser user, DBUserStat stat)
		{
			var fakearray = new int[3];
			var random = new Random();
			for (int i = 0; i < 3; i++)
				fakearray[i] = random.Next();
			var rankHistoryData = new osu.Game.Users.User.RankHistoryData
			{
				Mode = "osu",
				Data = fakearray
			};

			var kudosuCount = new osu.Game.Users.User.KudosuCount
			{
				Total = 0,
				Available = 0
			};
			
			/*var userAchievement = new osu.Game.Users.User.UserAchievement
			{

			};*/
			var userCover = new osu.Game.Users.User.UserCover
			{
				CustomUrl = null,
				Url = "https://osu.ppy.sh/images/headers/profile-covers/c1.jpg",
				Id = 1
			};
			/*var monthlyPlayCounts = new osu.Game.Users.User.UserHistoryCount
			{
				
			};
			var replaysWatchedCounts = new osu.Game.Users.User.UserHistoryCount
			{

			};*/
			var gameUser = new APIExtend.APIUser
			{
				Id = user.Id,
				JoinDate = new DateTimeOffset(TimeUtil.UnixTimeStampToDateTime(user.RegisterationDate.HasValue ? user.RegisterationDate.GetValueOrDefault() : 0)),
				Username = user.UserName,
				PreviousUsernames = new string[0],
				Country = new osu.Game.Users.Country
				{
					FullName = "South Korea",
					FlagName = "KR"
				},
				Colour = null,
				AvatarUrl = "images/layout/avatar-guest.png",
				CoverUrl = userCover.CustomUrl == null ? userCover.Url : userCover.CustomUrl,
				Cover = userCover,
				IsAdmin = false,
				IsSupporter = user.SupporterLevel > 0,
				IsGMT = false,
				IsQAT = false,
				IsBNG = false,
				IsBot = false,
				Active = false,
				IsOnline = false,
				PMFriendsOnly = user.IgnorePM,
				Interests = user.CustomInterest,
				Occupation = null,
				Title = null,
				Location = user.CustomLocations,
				LastVisit = user.LastVisit != null ? new DateTimeOffset(TimeUtil.UnixTimeStampToDateTime(user.LastVisit.GetValueOrDefault())) : null,
				Twitter = user.CustomTwitter,
				Skype = user.CustomSkype,
				Discord = user.CustomDiscord,
				Website = user.CustomWebsite,
				PostCount = 0,
				FollowerCount = 0,
				FavouriteBeatmapsetCount = 0,
				GraveyardBeatmapsetCount = 0,
				LovedBeatmapsetCount = 0,
				RankedAndApprovedBeatmapsetCount = 0,
				UnrankedBeatmapsetCount = 0,
				ScoresFirstCount = user.ScoreFirstCount,
				BeatmapPlaycountsCount = 0,
				PlayStyles = new osu.Game.Users.User.PlayStyle[0],
				PlayMode = "osu",
				ProfileOrder = new string[]
				{
					"top_ranks",
					"historical",
					"recent_activity",
					"medals",
					"beatmaps",
					"kudosu",
					"me"
				},
				Kudosu = kudosuCount,
				Statistics = ToUserStatistics(stat),
				Badges = new osu.Game.Users.Badge[0],
				Achievements = new osu.Game.Users.User.UserAchievement[0],
				MonthlyPlaycounts = new osu.Game.Users.User.UserHistoryCount[0],
				ReplaysWatchedCounts = new osu.Game.Users.User.UserHistoryCount[0],
				RankHistoryData = rankHistoryData
			};
			return gameUser;
		}
		public static osu.Game.Users.UserStatistics ToUserStatistics(this DBUserStat stat)
		{
			
			var levelinfo = new osu.Game.Users.UserStatistics.LevelInfo
			{
				Current = 1,
				Progress = 0
			}; // todo

			var ranks = new osu.Game.Users.UserStatistics.UserRanks
			{
				Global = stat.GlobalRank,
				Country = stat.CountryRank
			};

			var gradescount = new osu.Game.Users.UserStatistics.Grades
			{
				SSPlus = stat.XHCount,
				SS = stat.SSCount,
				SPlus = stat.SHCount,
				S = stat.SCount,
				A = stat.ACount
			};
			var userstats = new APIExtend.APIUserStatistics
			{
				IsRanked = stat.GlobalRank != null ? true : false,
				Level = levelinfo,
				PP = stat.Performance,
				Ranks = ranks,
				RankedScore = stat.RankedScore,
				Accuracy = stat.Accuracy,
				PlayCount = stat.PlayCount,
				PlayTime = stat.TotalPlayed,
				TotalScore = stat.TotalScore,
				TotalHits = stat.TotalHit,
				MaxCombo = stat.MaxCombo,
				ReplaysWatched = stat.ReplayCount,
				GradesCount = gradescount,
			};
			return userstats;
		}
	}
}
