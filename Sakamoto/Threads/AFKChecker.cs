using Sakamoto.Cache;
using Sakamoto.Objects;
using System;
using System.Collections.Generic;
using System.Timers;

namespace Sakamoto.Threads
{
	/// <summary>
	/// This class this AFKChecker.
	/// We need to log out users who are disconnected but are not sent to the server.
	/// </summary>
	public class AFKChecker
	{
		private static Timer timer;
		public static bool is_initalized = false;
		/// <summary>
		/// Initalize the timer. if already initalized, it will be do nothing.
		/// </summary>
		public static void Initalize()
		{
			if (is_initalized) return;
			timer = new Timer();
			timer.Elapsed += AFKCheck;
			timer.AutoReset = true;
			timer.Interval = 1000 * 500;
			timer.Start();
			is_initalized = true;
			Console.WriteLine("AFK Timer: Started");
		}
		/// <summary>
		/// Event of Timer. This void will check the afk users, and remove from <see cref="UserCache"/>.
		/// User have to relogin because they aren't in <see cref="UserCache"/>.
		/// </summary>
		private static void AFKCheck(Object source, ElapsedEventArgs e)
		{
			List<User> toremove = new List<User>();
			long current = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
			foreach (User u in UserCache.userlist)
			{
				if (u.type == Enums.PlayerType.Player)
				{
					if (u.lasttimestamp + (1000 * 400) > current)
					{
						Console.WriteLine($"AFK Checker: User {u.username} ({u.userid}) is disconnected");
						Console.WriteLine($"AFK Checker: Time Difference {(current - u.lasttimestamp) / 1000}s");
						toremove.Add(u);
					}
				}
			}
			foreach (User u in toremove)
				UserCache.userlist.Remove(u);

		}
	}
}
