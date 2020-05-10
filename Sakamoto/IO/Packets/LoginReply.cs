using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using Sakamoto.Enums;
using Sakamoto.Interfaces;

namespace Sakamoto.IO.Packets
{
   [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
   public class LoginReply : BasePacket
   {
      public const PacketId Id = PacketId.OutLoginReply;

      public int Reply { get; set; }

      public LoginReply(int reply)
      {
         Reply = reply;
      }

      public override void WriteTo(BinaryWriter writer)
      {
         using var dataStream = new MemoryStream(sizeof(int));
         using var dataWriter = new BinaryWriter(dataStream);

         dataWriter.Write(Reply);

         WriteInternal(writer, Id, dataStream.ToArray());
      }

      public override void ReadFrom(BinaryReader reader, PacketSerializationMode serializationMode = PacketSerializationMode.Full)
      {
         ReadInternal(reader, Id, serializationMode);

         using var dataStream = new MemoryStream(reader.ReadBytes(reader.ReadInt32()));
         using var dataReader = new BinaryReader(dataStream);

         Reply = dataReader.ReadInt32();
      }

      public static LoginReply FromReader(BinaryReader reader, PacketSerializationMode serializationMode = PacketSerializationMode.Full)
      {
         var loginReply = new LoginReply(int.MaxValue);

         loginReply.ReadFrom(reader, serializationMode);

         return loginReply;
      }
   }
}
