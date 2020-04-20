using HOPEless.Bancho;
using Sakamoto.Cache;
using Sakamoto.Objects;

namespace Sakamoto.Util
{
	public static class UserUtil
	{
		public static void SendAllUser(User u)
		{
			foreach (User i in UserCache.userlist)
			{
				u.AddQueue(new BanchoPacket(PacketType.ServerUserPresence, i.ToPresence()));
			}
		}
	}
}
