using Sakamoto.Cache;
using Sakamoto.Objects;
using System;
using System.Timers;

namespace Sakamoto.Threads
{

	public class AFKChecker
	{
		private static Timer timer;
		public static async void Initalize()
		{
			timer = new Timer();
			timer.Elapsed += AFKCheck;
			timer.AutoReset = true;
			timer.Interval = 1000 * 300;
			timer.Start();
			Console.WriteLine("AFK Timer: Started");
		}
		private static async void AFKCheck(Object source, ElapsedEventArgs e)
		{
			long current = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
			foreach (User u in UserCache.userlist)
			{
				if (u.type == Enums.PlayerType.Player)
				{
					if (u.lasttimestamp + (1000 * 350) > current)
					{
						Console.WriteLine($"AFK Checker: User {u.username} ({u.userid}) is disconnected");
						UserCache.Remove(u);
					}
				}
			}
		}
	}
}
