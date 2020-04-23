using Sakamoto.Objects;
using System.Collections.Generic;
using System.Linq;

namespace Sakamoto.Cache
{
	public class DummyUserCache
	{
		public static class OnlineUserCache
		{
			// This list is online lists.
			public static List<DummyUser> userlist = new List<DummyUser>();
			public static DummyUser GetUserById(int id)
			{
				return userlist.Where(a => a.id == id).FirstOrDefault();
			}
			public static DummyUser GetUserByName(string name)
			{
				return userlist.Where(a => a.username.ToLower() == name.ToLower()).FirstOrDefault();
			}

			public static void Add(DummyUser user)
			{
				if (GetUserById(user.id) != null && GetUserByName(user.username) == null) return;
				if (GetUserById(user.id) != null)
					Remove(GetUserById(user.id));
				userlist.Add(user);
			}
			public static void Remove(DummyUser user)
			{
				userlist.Remove(user);
			}

		}
	}
}
