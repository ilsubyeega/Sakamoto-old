using HOPEless.Bancho;
using Sakamoto.Events.Packet;
using Sakamoto.Objects;
using System;
namespace Sakamoto.Events
{
	public static class PacketEventHandler
	{
		public static void Handle(BanchoPacket packet, User user)
		{
			switch (packet.Type)
			{
				default:
					Console.WriteLine("Not Handled this Packet: " + packet.Type);
					break;
				case PacketType.ClientDisconnect:
					ClientDisconnect.Handle(packet, user);
					break;
				case PacketType.ClientPong:
					ClientPong.Handle(packet, user);
					break;
				case PacketType.ClientUserStatus:
					ClientUserStatus.Handle(packet, user);
					break;
				case PacketType.ClientUserStatsRequest:
					ClientUserStatsRequest.Handle(packet, user);
					break;
			}

		}
	}
}
