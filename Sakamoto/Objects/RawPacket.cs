using Sakamoto.Enums;

namespace Sakamoto.Objects
{
	class RawPacket
	{
		public RawPacket(short type, int length, byte[] bytearray)
		{
			this.type = type;
			this.length = length;
			this.bytearray = bytearray;
		}

		public short type;
		public int length;
		public byte[] bytearray;

		public PacketType getType()
		{
			return (PacketType)type;
		}

		public string toString()
		{
			return "Type: " + (PacketType)type + " Length: " + length;
		}
	}
}
