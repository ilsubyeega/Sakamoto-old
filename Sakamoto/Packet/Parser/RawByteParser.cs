using osu.Shared.Serialization;
using Sakamoto.Objects;
using System.Collections.Generic;
using System.IO;

namespace Sakamoto.Packet.Parser
{
	class RawByteParser
	{
		public static List<RawPacket> Parse(byte[] bytearray)
		{
			List<RawPacket> list = new List<RawPacket>();
			try
			{
				var stream = new MemoryStream(bytearray);
				SerializationReader r = new SerializationReader(stream);
				while (r.BaseStream.Position != r.BaseStream.Length)
				{
					short type = r.ReadInt16();
					int length = r.ReadInt32();
					list.Add(new RawPacket(type, length, r.ReadBytes(length)));
				}
				return list;
			}
			catch
			{
				return null;
			}
		}
	}
}
