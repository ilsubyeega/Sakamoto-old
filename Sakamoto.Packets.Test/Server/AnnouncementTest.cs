using System.IO;

using NUnit.Framework;

using Sakamoto.Packets.Extensions;
using Sakamoto.Packets.Server;

namespace Sakamoto.Packets.Test.Server
{
   public partial class ServerPacketTests
   {
      [TestCase("Hello, world!", Description = "Simple plain english characters encoding test")]
      [TestCase("UTF-8 인코딩 테스트", Description = "Test that involves UTF-8 encoded characters")]
      public void AnnouncementTest(string message)
      {
         using var stream = new MemoryStream();
         using var writer = new BinaryWriter(stream);
         using var reader = new BinaryReader(stream);

         writer.WritePacket(new Announcement(message));

         stream.Position = 0;

         var packet = reader.ReadPacket();

         Assert.IsNotNull(packet);
         Assert.IsTrue(packet.GetType() == typeof(Announcement));
         Assert.AreEqual(((Announcement) packet).Message, message);
      }
   }
}
