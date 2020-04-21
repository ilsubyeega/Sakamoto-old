using HOPEless.Bancho;
using HOPEless.Bancho.Objects;
using Sakamoto.Cache;
using Sakamoto.Manager;
using Sakamoto.Objects;
using System;

namespace Sakamoto.Events.Packet
{
	public static class ClientChatMessagePublic
	{
		public static void Handle(BanchoPacket packet, User user)
		{
			BanchoChatMessage msg = new BanchoChatMessage(packet.Data);
			Console.WriteLine($"Chat: {user.username} => {msg.Channel} : {msg.Message}");
			if (!ChatManager.JoinedChannel(msg.Channel, user))
			{
				user.AddQueue(new BanchoPacket(PacketType.ServerChatChannelRevoked, new BanchoString(msg.Channel)));
				Console.WriteLine($"{user.username} tried to send message to {msg.Channel}, but he didnt join that channel.");
				return;
			}
			if (msg.Message.StartsWith("!"))
				BotCommandEvent.Handle(user, msg);
			ChatManager.SendMessage(msg.Channel, user, msg.Message);
			return;
			
			
		}
	}
}
