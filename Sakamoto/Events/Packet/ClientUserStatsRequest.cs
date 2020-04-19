using HOPEless.Bancho;
using Sakamoto.Cache;
using Sakamoto.Objects;

namespace Sakamoto.Events.Packet
{
	public static class ClientUserStatsRequest
	{
		public static void Handle(BanchoPacket packet, User u)
		{
			/* BanchoIntList list = new BanchoIntList(packet.Data);
			foreach (int playerid in list.Value)
			{
				User i = UserCache.GetUserById(playerid);
				if (i != null)
					u.addQueue(new BanchoPacket(PacketType.ServerUserData, i.ToUserData()));
			}*/
			foreach (User i in UserCache.userlist)
			{
				u.addQueue(new BanchoPacket(PacketType.ServerUserData, i.ToUserData()));
			}

		}
	}
}
