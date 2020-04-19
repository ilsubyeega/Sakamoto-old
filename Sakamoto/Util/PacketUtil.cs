using HOPEless.Bancho;
using osu.Shared.Serialization;
using System.Collections.Generic;

namespace Sakamoto.Util
{
	public static class PacketUtil
	{
		public static void WriteToStream(List<BanchoPacket> packets, SerializationWriter writer)
		{
			WriteToStream(packets.ToArray(), writer);
		}
		public static void WriteToStream(BanchoPacket[] packets, SerializationWriter writer)
		{
			foreach (BanchoPacket packet in packets)
			{
				packet.WriteToStream(writer);
			}
		}
	}
}
