using osu.Shared.Serialization;
using Sakamoto.Packet.Enums;

namespace Sakamoto.Packet.Objects
{
	public class Packet
	{
		public PacketType type;
		public int length;
		public byte[] bytearray;
		public Packet()
		{

		}

		public Packet(RawPacket rawpacket)
		{
			this.type = rawpacket.type;
			this.length = rawpacket.length;
			this.bytearray = rawpacket.bytearray;
		}

		public void WriteRawToStream(SerializationWriter writer)
		{
			writer.Write((short)type);
			writer.Write(0x00);
			writer.Write(length);
			writer.WriteRaw(bytearray);
		}
		public void WriteRawToStream(SerializationWriter writer, byte[] bytearray)
		{
			writer.Write((short)type);
			writer.Write(0x00);
			writer.Write(length);
			writer.WriteRaw(bytearray);
		}
	}
}
