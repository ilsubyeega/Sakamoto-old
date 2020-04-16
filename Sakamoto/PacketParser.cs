using HOPEless.Bancho;
using osu.Shared.Serialization;
using System.Collections.Generic;
using System.IO;

namespace Sakamoto
{
	public class PacketParser
	{
		public static List<BanchoPacket> Parse(MemoryStream stream)
		{
			List<BanchoPacket> list = new List<BanchoPacket>();
			try
			{
				SerializationReader r = new SerializationReader(stream);
				while (r.BaseStream.Position != r.BaseStream.Length)
					list.Add(new BanchoPacket(r));
				return list;
			}
			catch
			{
				return null;
			}
		}
	}
}
