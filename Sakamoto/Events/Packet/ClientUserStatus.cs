using HOPEless.Bancho;
using HOPEless.Bancho.Objects;
using Sakamoto.Objects;
using System;

namespace Sakamoto.Events.Packet
{
	public static class ClientUserStatus
	{
		public static void Handle(BanchoPacket packet, User user)
		{
			BanchoUserStatus status = new BanchoUserStatus(packet.Data);
			
			user.status = status;
			Console.WriteLine($"{user.status.PlayMode}");
			user.addQueue(new BanchoPacket(PacketType.ServerUserData, user.status));
		}
	}
}
