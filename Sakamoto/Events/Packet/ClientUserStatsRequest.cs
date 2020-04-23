using HOPEless.Bancho;
using HOPEless.Bancho.Objects;
using Sakamoto.Cache;
using Sakamoto.Objects;

namespace Sakamoto.Events.Packet
{
	public static class ClientUserStatsRequest
	{
		public static void Handle(BanchoPacket packet, User u)
		{
			BanchoIntList list = new BanchoIntList(packet.Data);
			foreach (int inp in list.Value)
			{
				User a = OnlineUserCache.GetUserById(inp);
				if (a == null)
				{
					u.AddQueue(new BanchoPacket(PacketType.ServerUserQuit, new BanchoUserQuit(inp)));
				}
				else
				{
					u.AddQueue(new BanchoPacket(PacketType.ServerUserData, a.ToUserData()));
				}
			}

		}
	}
}
