using System;
using System.IO;
using System.Text;
using Sakamoto.Enums;
using Sakamoto.Interfaces;
using Sakamoto.IO.Packets;

namespace Sakamoto.Extensions
{
   public static class BinaryReaderExtensions
   {
      public static ulong ReadUleb(this BinaryReader @this)
      {
         var result = 0ul;
         var shift = 0;

         while (true)
         {
            var @byte = @this.ReadByte();

            result |= (ulong) (@byte & 0x7F) << shift;
            shift += 7;

            if ((@byte & 0x80) == 0)
               break;
         }

         return result;
      }

      public static string ReadOsuString(this BinaryReader @this)
      {
         var tag = @this.ReadByte();

         if (tag != 0xB)
            return string.Empty;

         var length = @this.ReadUleb();
         var bytes = @this.ReadBytes((int) length);

         return Encoding.UTF8.GetString(bytes);
      }

      public static IPacket ReadPacket(this BinaryReader @this)
      {
         if (@this.BaseStream.Length - @this.BaseStream.Position < 7)
            return null;

         var packetId = (PacketId) @this.ReadInt16();

         return packetId switch
         {
            PacketId.OutLoginReply => LoginReply.FromReader(@this, PacketSerializationMode.ReadCompression),
            PacketId.OutAnnouncement => Announcement.FromReader(@this, PacketSerializationMode.ReadCompression),

            _ => null
         };
      }
   }
}
