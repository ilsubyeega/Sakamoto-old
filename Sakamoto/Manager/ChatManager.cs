using HOPEless.Bancho;
using HOPEless.Bancho.Objects;
using Sakamoto.Cache;
using Sakamoto.Objects;
using System.Collections.Generic;
using System.Linq;

namespace Sakamoto.Manager
{
	public static class ChatManager
	{
		public static List<Channel> channels = new List<Channel>();
		public static bool Initalized = false;
		public static void Init()
		{
			if (Initalized) return;
			channels.Add(new Channel()
			{
				name = "#osu",
				description = "Welcome to osu!",
				autojoin = true
			});
			channels.Add(new Channel()
			{
				name = "#korean",
				description = "Korean only chat.",
				autojoin = true
			});
			channels.Add(new Channel()
			{
				name = "#dev",
				description = "Developer's Chat",
				autojoin = true
			});
			Initalized = true;
		}
		public static void JoinChannel(string name, int userid)
		{
			User u = UserCache.GetUserById(userid);
			if (u == null) return;
			if (JoinedChannel(name, userid)) return;
			if (!HasChannel(name)) return;
			channels
				.Where(instance => instance.name == name)
				.ToList()
				.ForEach(instance => instance.playerlist.Add(userid));
			u.AddQueue(new BanchoPacket(PacketType.ServerChatChannelJoinSuccess, new BanchoString(name)));
		}
		public static void LeaveChannel(string name, int userid)
		{
			User u = UserCache.GetUserById(userid);
			if (u == null) return;
			if (!JoinedChannel(name, userid)) return;
			channels
				.Where(instance => instance.name == name)
				.ToList()
				.ForEach(instance => instance.playerlist.Remove(userid));
			u.AddQueue(new BanchoPacket(PacketType.ServerChatChannelRevoked, new BanchoString(name)));
		}
		public static bool JoinedChannel(string name, int userid)
		{
			return channels.Any(a => a.name == name && a.playerlist.Contains(userid));
		}
		public static bool HasChannel(string name)
		{
			return channels.Any(a => a.name == name);
		}
		public static Channel GetChannel(string name)
		{
			return channels.Where(a => a.name == name).FirstOrDefault();
		}
		public static Channel[] GetAutoJoinChannel(bool isauto)
		{
			return channels.Where(a => a.autojoin == isauto).ToArray();
		}
		public static Channel[] GetListJoinedChannel(int userid)
		{
			return channels.Where(a => a.playerlist.Contains(userid)).ToArray();
		}

		public static void SendMessage(string name, int userid, string message)
		{
			Channel channel = GetChannel(name);
			if (channel == null) return;
			if (!JoinedChannel(name, userid)) return;
			// Todo queue every joined member
		}
		public static void SendMessageSecret(string name, int userid)
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
		public short GetUserCount()
		{
			return (short)playerlist.Count();
		}
		public List<int> playerlist = new List<int>();
	}
}
