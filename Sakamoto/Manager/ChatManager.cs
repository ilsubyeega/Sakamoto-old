using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Manager
{
	public static class ChatManager
	{
		public static List<Channel> channels = new List<Channel>();
		public static bool Initalized = false;
		public static void Init()
		{
			channels.Add(new Channel()
			{
				name = "general",
				description = "General Chat",
				autojoin = true
			});
			channels.Add(new Channel()
			{
				name = "dev",
				description = "Developer's Chat",
				autojoin = true
			});
			Initalized = true;
		}
		public static void joinChannel(string name, int userid)
		{
			if (JoinedChannel(name, userid)) return;
			channels
				.Where(instance => instance.name == name)
				.ToList()
				.ForEach(instance => instance.playerlist.Add(userid));
		}
		public static void LeaveChannel(string name, int userid)
		{
			if (!JoinedChannel(name, userid)) return;
			channels
				.Where(instance => instance.name == name)
				.ToList()
				.ForEach(instance => instance.playerlist.Remove(userid));
		}
		public static bool JoinedChannel(string name, int userid)
		{
			return channels.Any(a => a.name == "name" && a.playerlist.Contains(userid));
		}
		public static bool HasChannel(string name)
		{
			return channels.Any(a => a.name == "name");
		}
		public static Channel GetChannel(string name)
		{
			return channels.Where(a => a.name == "name").FirstOrDefault();
		}

		public static void SendMessage(string name, int userid)
		{
			Channel channel = GetChannel(name);
			if (channel == null) return;
			if (!JoinedChannel(name, userid)) return;
			// Todo queue every joined member

		}
	}

	public class Channel
	{
		public string name;
		public string description;
		public bool autojoin = false;
		public List<int> playerlist = new List<int>();
	}
}
