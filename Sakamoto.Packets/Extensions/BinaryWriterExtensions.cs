using System.IO;
using System.Text;

using Sakamoto.Packets.Enums;
using Sakamoto.Packets.Interfaces;

namespace Sakamoto.Packets.Extensions
{
   public static class BinaryWriterExtensions
   {
      public static void WriteUleb(this BinaryWriter @this, ulong value)
      {
         do
         {
            var @byte = (byte) (value & 0x7F);

            if ((value >>= 7) != 0)
               @byte |= 0x80;

            @this.Write(@byte);
         }
         while (value > 0);
      }

      public static void WriteOsuString(this BinaryWriter @this, string value,
         StringSerializationMode serializationMode = StringSerializationMode.Full)
      {
         value ??= "";

         if (!string.IsNullOrEmpty(value) || serializationMode == StringSerializationMode.Full)
         {
            var bytes = Encoding.UTF8.GetBytes(value);

            @this.Write((byte) 0xB);
            @this.WriteUleb((ulong) bytes.Length);
            @this.Write(bytes);
         }
         else
            @this.Write((byte) 0);
      }

      public static void WritePacket(this BinaryWriter @this, IPacket packet) => packet.WriteTo(@this);
   }
}
