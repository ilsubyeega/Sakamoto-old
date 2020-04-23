using HOPEless.Bancho;
using Sakamoto.Cache;
using Sakamoto.Objects;
using System;

namespace Sakamoto.Events.Packet
{
	public static class ClientDisconnect
	{
		public static void Handle(BanchoPacket packet, User user)
		{
			OnlineUserCache.Remove(user);
			Console.WriteLine($"{user.username} ({user.userid}) got disconnected.");
		}
	}
}
