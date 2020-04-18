using HOPEless.Bancho;
using osu.Shared.Serialization;
using Sakamoto.Events.Packet;
using System;
namespace Sakamoto.Events
{
	public static class PacketEventHandler
	{
		public static void Handle(BanchoPacket packet, SerializationWriter writer)
		{
			switch (packet.Type)
			{
				default:
					Console.WriteLine("Not Handled this Packet: " + packet.Type);
					break;
				case PacketType.ClientUserStatus:
					ClientUserStatus.Handle(packet, writer);
					break;
			}

		}
	}
}
