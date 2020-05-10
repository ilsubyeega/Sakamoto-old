using System.Diagnostics;
using System.IO;

using Sakamoto.Enums;
using Sakamoto.Interfaces;

namespace Sakamoto.IO
{
   public abstract class BasePacket : IPacket
   {
      protected void ReadInternal(BinaryReader reader, PacketId id,
         PacketSerializationMode serializationMode = PacketSerializationMode.Full)
      {
         if ((serializationMode & PacketSerializationMode.ReadId) > 0)
         {
            var packetId = reader.ReadInt16();

            Debug.Assert(packetId == (short) id);
         }

         if ((serializationMode & PacketSerializationMode.ReadCompression) > 0)
            reader.ReadByte();
      }

      protected void WriteInternal(BinaryWriter writer, PacketId id, byte[] bodyData)
      {
         writer.Write((short) id);
         writer.Write((byte) 0);
         writer.Write(bodyData.Length);
         writer.Write(bodyData);
      }

      public abstract void WriteTo(BinaryWriter writer);
      public abstract void ReadFrom(BinaryReader reader, PacketSerializationMode serializationMode = PacketSerializationMode.Full);
   }
}
