using HOPEless.Bancho;
using HOPEless.Bancho.Objects;
using Sakamoto.Cache;
using Sakamoto.Objects;
using System;

namespace Sakamoto.Events.Packet
{
	public class ClientRequestPlayerList
	{
		public static void Handle(BanchoPacket packet, User user)
		{
			BanchoInt status = new BanchoInt(packet.Data);
			Console.WriteLine("s: " + status.Value);
			foreach (User i in UserCache.userlist)
			{
				user.AddQueue(new BanchoPacket(PacketType.ServerUserPresence, i.ToPresence()));
			}
		}
	}
}
