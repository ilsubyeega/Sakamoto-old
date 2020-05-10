using System.IO;

using NUnit.Framework;

using Sakamoto.Packets.Extensions;
using Sakamoto.Packets.Server;

namespace Sakamoto.Packets.Test.Server
{
   public partial class ServerPacketTests
   {
      [TestCase(0, Description = "Encoding an integer value 0")]
      [TestCase(-1, Description = "Encoding an integer value -1")]
      [TestCase(1, Description = "Encoding an integer value 1")]
      [TestCase(int.MinValue, Description = "Encoding the lowest integer value")]
      [TestCase(int.MaxValue, Description = "Encoding the highest integer value")]
      public void LoginReplyTest(int reply)
      {
         using var stream = new MemoryStream();
         using var writer = new BinaryWriter(stream);
         using var reader = new BinaryReader(stream);

         writer.WritePacket(new LoginReply(reply));

         stream.Position = 0;

         var packet = reader.ReadPacket();

         Assert.IsNotNull(packet);
         Assert.IsTrue(packet.GetType() == typeof(LoginReply));
         Assert.AreEqual(((LoginReply) packet).Reply, reply);
      }
   }
}
