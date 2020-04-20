using HOPEless.Bancho;
using Sakamoto.Cache;
using Sakamoto.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Util
{
	public static class UserUtil
	{
		public static void sendAllUser(User u)
		{
			foreach (User i in UserCache.userlist)
			{
				u.addQueue(new BanchoPacket(PacketType.ServerUserPresence, i.ToPresence()));
			}
		}
	}
}
