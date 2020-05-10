using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using Sakamoto.Enums;
using Sakamoto.Extensions;

namespace Sakamoto.IO.Packets
{
   [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
   public class Announcement : BasePacket
   {
      public const PacketId Id = PacketId.OutAnnouncement;

      public string Message { get; set; }

      public Announcement(string message = "")
      {
         Message = message;
      }

      public override void WriteTo(BinaryWriter writer)
      {
         using var dataStream = new MemoryStream();
         using var dataWriter = new BinaryWriter(dataStream);

         dataWriter.WriteOsuString(Message);

         WriteInternal(writer, Id, dataStream.ToArray());
      }

      public override void ReadFrom(BinaryReader reader, PacketSerializationMode serializationMode = PacketSerializationMode.Full)
      {
         ReadInternal(reader, Id, serializationMode);

         using var dataStream = new MemoryStream(reader.ReadBytes(reader.ReadInt32()));
         using var dataReader = new BinaryReader(dataStream);

         Message = dataReader.ReadOsuString();
      }

      public static Announcement FromReader(BinaryReader reader, PacketSerializationMode serializationMode = PacketSerializationMode.Full)
      {
         var announcement = new Announcement();

         announcement.ReadFrom(reader, serializationMode);

         return announcement;
      }
   }
}
