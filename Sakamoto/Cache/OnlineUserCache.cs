using Sakamoto.Objects;
using System.Collections.Generic;
using System.Linq;

namespace Sakamoto.Cache
{
	public static class OnlineUserCache
	{
		// This list is online lists.
		public static List<User> userlist = new List<User>();
		public static User GetUserById(int id)
		{
			return userlist.Where(a => a.userid == id).FirstOrDefault();
		}
		public static User GetUserByName(string name)
		{
			return userlist.Where(a => a.username.ToLower() == name.ToLower()).FirstOrDefault();
		}
		public static User GetUserByToken(string token)
		{
			return userlist.Where(a => a.chotoken == token).FirstOrDefault();
		}

		public static void Add(User user)
		{
			if (GetUserById(user.userid) != null && GetUserByName(user.username) == null) return;
			if (GetUserById(user.userid) != null)
				Remove(GetUserById(user.userid));
			userlist.Add(user);
		}
		public static void Remove(User user)
		{
			userlist.Remove(user);
		}

	}
}
