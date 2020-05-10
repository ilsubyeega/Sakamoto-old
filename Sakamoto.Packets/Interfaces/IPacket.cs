using System.IO;

using Sakamoto.Packets.Enums;

namespace Sakamoto.Packets.Interfaces
{
   public interface IPacket
   {
      void WriteTo(BinaryWriter writer);
      void ReadFrom(BinaryReader reader, PacketSerializationMode serializationMode = PacketSerializationMode.Full);
   }
}
