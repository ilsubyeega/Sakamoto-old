using System;

namespace Sakamoto.Enums
{
   [Flags]
   public enum PacketSerializationMode
   {
      ReadId = 1 << 1,
      ReadCompression = 1 << 2,
      Full = ReadId | ReadCompression,
   }
}
