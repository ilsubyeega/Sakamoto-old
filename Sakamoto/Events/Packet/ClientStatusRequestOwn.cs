using HOPEless.Bancho;
using Sakamoto.Objects;

namespace Sakamoto.Events.Packet
{
	public static class ClientStatusRequestOwn
	{
		public static void Handle(BanchoPacket packet, User user)
		{
			user.AddQueue(new BanchoPacket(PacketType.ServerUserData, user.ToUserData()));
		}
	}
}
