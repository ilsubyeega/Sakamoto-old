using HOPEless.Bancho;
using Sakamoto.Cache;
using Sakamoto.Objects;

namespace Sakamoto.Util
{
	public static class UserUtil
	{
		public static void SendAllUser(User u)
		{
			foreach (User i in OnlineUserCache.userlist)
			{
				u.AddQueue(new BanchoPacket(PacketType.ServerUserPresence, i.ToPresence()));
			}
		}
	}
}
