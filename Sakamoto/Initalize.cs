using Microsoft.EntityFrameworkCore.Update.Internal;
using Sakamoto.Cache;
using Sakamoto.Objects;
using Sakamoto.Util;

namespace Sakamoto
{
	public static class Initalize
	{
		public static void Init()
		{
			Timer();
			Chat();
			CreateBot();
		}
		public static void CreateBot()
		{
			User bot = new User();
			bot.username = Common.bot_name;
			bot.username_safe = Common.bot_name.ToLower();
			bot.userid = Common.bot_userid;
			bot.chotoken = (TokenGenerator.Generate() + "bot");
			bot.type = Enums.PlayerType.Bot;
			bot.osuperm = osu.Shared.PlayerRank.Tournament;
			bot.status.Action = HOPEless.osu.BanchoAction.Modding;
			bot.status.ActionText = "Keesu";
			bot.status.PlayMode = (byte)osu.Shared.GameMode.Standard;
			UserCache.Add(bot);
		}
		public static void Chat() => Manager.ChatManager.Init();
		public static void Timer() => Threads.AFKChecker.Initalize();
	}
}
