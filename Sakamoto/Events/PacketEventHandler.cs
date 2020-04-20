using HOPEless.Bancho;
using Sakamoto.Objects;
using System;
namespace Sakamoto.Events
{
	public static class PacketEventHandler
	{
		public static void Handle(BanchoPacket packet, User user)
		{
			if (packet.Type != PacketType.ClientUserStatsRequest && packet.Type != PacketType.ClientPong)
				Console.WriteLine($"{packet.Type}");
			Type class_type = Type.GetType($"Sakamoto.Events.Packet.{packet.Type}");
			if (class_type == null || class_type.GetMethod("Handle") == null)
			{
				Console.WriteLine($"Not Handled this Packet: {packet.Type}");
				return;
			}
			class_type.GetMethod("Handle").Invoke(null, new object[] { packet, user });
		}
	}
}
