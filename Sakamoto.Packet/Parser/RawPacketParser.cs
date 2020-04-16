using osu.Shared.Serialization;
using Sakamoto.Packet.Enums;
using Sakamoto.Packet.Objects;
using System.Collections.Generic;
using System.IO;

namespace Sakamoto.Packet.Parser
{
	public class RawPacketParser
	{
		public static List<RawPacket> Parse(MemoryStream stream)
		{
			List<RawPacket> list = new List<RawPacket>();
			try
			{
				SerializationReader r = new SerializationReader(stream);
				while (r.BaseStream.Position != r.BaseStream.Length)
				{
					short type = r.ReadInt16();
					r.ReadByte();
					int length = r.ReadInt32();
					list.Add(new RawPacket((PacketType)type, length, r.ReadBytes(length)));
				}
				return list;
			}
			catch
			{
				return null;
			}
		}
		public static bool isValid(MemoryStream stream)
		{
			try
			{
				SerializationReader r = new SerializationReader(stream);
				while (r.BaseStream.Position != r.BaseStream.Length)
				{
					r.ReadInt16();
					if (r.ReadByte() != 0x00)
						return false;
					int length = r.ReadInt32();
					r.ReadBytes(length);
				}
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}
