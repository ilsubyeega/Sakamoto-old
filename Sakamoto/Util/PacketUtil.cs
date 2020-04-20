using HOPEless.Bancho;
using osu.Shared.Serialization;
using System.Collections.Generic;

namespace Sakamoto.Util
{
	/// <summary>
	/// This class writes <see cref="BanchoPacket"/> arrays.
	/// </summary>
	public static class PacketUtil
	{
		/// <summary>
		/// Write <paramref name="packets"/> to <paramref name="writer"/>.
		/// </summary>
		/// <param name="packets">List of packets. Can be array of packets.</param>
		/// <param name="writer">Writer</param>
		public static void WriteToStream(List<BanchoPacket> packets, SerializationWriter writer)
		{
			WriteToStream(packets.ToArray(), writer);
		}
		/// <summary>
		/// Write <paramref name="packets"/> to <paramref name="writer"/>.
		/// </summary>
		/// <param name="packets">Array of packets.</param>
		/// <param name="writer">Writer</param>
		public static void WriteToStream(BanchoPacket[] packets, SerializationWriter writer)
		{
			foreach (BanchoPacket packet in packets)
			{
				packet.WriteToStream(writer);
			}
		}
	}
}
