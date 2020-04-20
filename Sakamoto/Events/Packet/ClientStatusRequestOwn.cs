using HOPEless.Bancho;
using Sakamoto.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Events.Packet
{
	public static class ClientStatusRequestOwn
	{
		public static void Handle(BanchoPacket packet, User user)
		{
			user.addQueue(new BanchoPacket(PacketType.ServerUserData, user.ToUserData()));
		}
	}
}
