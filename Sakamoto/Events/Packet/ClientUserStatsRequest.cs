using HOPEless.Bancho;
using HOPEless.Bancho.Objects;
using Sakamoto.Cache;
using Sakamoto.Objects;
using System;

namespace Sakamoto.Events.Packet
{
	public static class ClientUserStatsRequest
	{
		public static void Handle(BanchoPacket packet, User u)
		{
			BanchoIntList list = new BanchoIntList(packet.Data);
			foreach (int inp in list.Value) {
				u.addQueue(new BanchoPacket(PacketType.ServerUserData, UserCache.GetUserById(inp).ToUserData()));
			}

		}
	}
}
