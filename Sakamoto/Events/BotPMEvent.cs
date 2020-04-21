using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sakamoto.Objects;
using HOPEless.Bancho.Objects;
using HOPEless.Bancho;

namespace Sakamoto.Events
{
	public static class BotPMEvent
	{
		public static void Handle(User user, BanchoChatMessage message)
		{
			try
			{
				string[] msg = message.Message.Split(' ');
				if (msg[0].StartsWith("\x1") && msg[0].EndsWith("ACTION"))
				{
					if (msg[4].Contains("https://osu.ppy.sh/b/"))
					{
						int b_id = int.Parse(msg[4].Substring(22));
						SendMsg(user, $"Beatmap Information\nBeatmap: {GetBeatmapMsgFromStringList(msg)}  ({b_id})");
						
					}
				}
			} catch { }
		}
		private static string GetBeatmapMsgFromStringList(string[] a)
		{
			List<string> _tmp = a.ToList();
			_tmp.RemoveRange(0, 5);
			return String.Join(" ", _tmp).Substring(0, String.Join(" ", _tmp).Length - 2);
		}
		private static void SendMsg(User user, String message) => user.AddQueue(new BanchoPacket(
			PacketType.ServerChatMessage, 
			new BanchoChatMessage(
				Common.bot_name, 
				message, 
				user.username, 
				Common.bot_userid)
			)
		);
	}
}
