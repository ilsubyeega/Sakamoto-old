using System;
using System.IO;

using Sakamoto.Extensions;
using Sakamoto.IO.Packets;

namespace Sakamoto
{
   public static class Program
   {
      public static void Main(string[] args)
      {
         using var stream = new MemoryStream();
         using var writer = new BinaryWriter(stream);

         writer.WritePacket(new LoginReply(-1));
         writer.WritePacket(new Announcement("Hello, world!"));

         stream.Position = 0;

         using var reader = new BinaryReader(stream);

         var loginReply = reader.ReadPacket();
         var announcement = reader.ReadPacket();

         if (loginReply?.GetType() == typeof(LoginReply))
            Console.WriteLine($"Login reply: {((LoginReply) loginReply).Reply}");

         if (announcement?.GetType() == typeof(Announcement))
            Console.WriteLine($"Announcement: {((Announcement) announcement).Message}");
      }
   }
}
