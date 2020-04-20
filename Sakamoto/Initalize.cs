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
			bot.username = "Sakamoto";
			bot.username_safe = "sakamoto";
			bot.userid = 2;
			bot.chotoken = (TokenGenerator.Generate() + "bot");
			bot.type = Enums.PlayerType.Bot;
			bot.status.Action = HOPEless.osu.BanchoAction.Modding;
			bot.status.ActionText = "Keesu";
			bot.status.PlayMode = (byte)osu.Shared.GameMode.Standard;
			UserCache.Add(bot);
		}
		public static void Chat() => Manager.ChatManager.Init();
		public static void Timer() => Threads.AFKChecker.Initalize();
	}
}
