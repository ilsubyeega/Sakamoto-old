using HOPEless.Bancho;
using osu.Shared.Serialization;
using System;
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
		public static void Debug(MemoryStream stream)
		{
			List<BanchoPacket> debug1 = Parse(stream);
			if (debug1 == null) return;
			foreach (BanchoPacket packet in debug1)
			{
				Console.WriteLine(packet.ToString());
			}
		}
		public static void Debug(List<BanchoPacket> packets)
		{
			if (packets == null) return;
			foreach (BanchoPacket packet in packets)
			{
				Console.WriteLine(packet.ToString());
			}
		}
	}
}
