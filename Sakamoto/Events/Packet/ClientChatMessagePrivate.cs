using HOPEless.Bancho;
using HOPEless.Bancho.Objects;
using Sakamoto.Cache;
using Sakamoto.Manager;
using Sakamoto.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Events.Packet
{
	public static class ClientChatMessagePrivate
	{
		public static void Handle(BanchoPacket packet, User user)
		{
			BanchoChatMessage msg = new BanchoChatMessage(packet.Data);
			Console.WriteLine($"Chat: {user.username} => {msg.Channel} : {msg.Message}");
			if (msg.Channel == Common.bot_name)
			{
				BotPMEvent.Handle(user, msg);
				return;
			}
			User to = OnlineUserCache.GetUserByName(msg.Channel);
			// User is offline.
			if (to == null)
			{
				ChatManager.RevokeChannel(msg.Channel, user);
				return;
			}
			ChatManager.SendMessageSecret(user, to, msg.Message);
		}
	}
}
