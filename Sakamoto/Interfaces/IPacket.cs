using System.IO;

using Sakamoto.Enums;

namespace Sakamoto.Interfaces
{
   public interface IPacket
   {
      void WriteTo(BinaryWriter writer);
      void ReadFrom(BinaryReader reader, PacketSerializationMode serializationMode = PacketSerializationMode.Full);
   }
}
