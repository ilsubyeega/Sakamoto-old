using HOPEless.Bancho;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sakamoto.Events.Packet;
namespace Sakamoto.Events
{
	public static class PacketEventHandler
	{
		public static void Handle(BanchoPacket packet)
		{
			switch (packet.Type)
			{
				default:
					Console.WriteLine("Not Handled this Packet: " + packet.Type);
					break;
				case PacketType.ClientUserStatus:
					ClientUserStatus.Handle(packet);
					break;
			}
				
		}
	}
}
