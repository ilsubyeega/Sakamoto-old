using osu.Shared.Serialization;
using Sakamoto.Packet.Enums;
using System.IO;

namespace Sakamoto.Packet.Objects
{
	public class RawPacket
	{
		public RawPacket(PacketType type, int length, byte[] bytearray)
		{
			this.type = type;
			this.length = length;
			this.bytearray = bytearray;
		}

		public PacketType type;
		public int length;
		public byte[] bytearray;

		public PacketType getType()
		{
			return type;
		}

		public override string ToString()
		{
			return "Type: " + type + " Length: " + length;
		}

		public byte[] toByteArray()
		{
			try
			{
				MemoryStream ms = new MemoryStream();
				SerializationWriter writer = new SerializationWriter(ms);
				writer.Write((short)type);
				writer.Write(0x00); // null bytes
				writer.WriteRaw(bytearray);
				return ms.ToArray();
			}
			catch
			{
				return null;
			}
		}

		public void WriteToStream(SerializationWriter writer)
		{
			writer.Write((short)type);
			writer.Write(0x00); // null bytes
			writer.Write(length);
			writer.WriteRaw(bytearray);
		}
	}
}
