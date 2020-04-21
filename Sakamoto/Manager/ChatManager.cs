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
		public static void JoinChannel(string name, User u)
		{
			if (u == null) return;
			if (JoinedChannel(name, u)) return;
			if (!HasChannel(name)) return;
			channels
				.Where(instance => instance.name == name)
				.ToList()
				.ForEach(instance => instance.playerlist.Add(u));
			u.AddQueue(new BanchoPacket(PacketType.ServerChatChannelJoinSuccess, new BanchoString(name)));
		}
		public static void LeaveChannel(string name, User u)
		{
			if (u == null) return;
			if (!JoinedChannel(name, u)) return;
			channels
				.Where(instance => instance.name == name)
				.ToList()
				.ForEach(instance => instance.playerlist.Remove(u));
			u.AddQueue(new BanchoPacket(PacketType.ServerChatChannelRevoked, new BanchoString(name)));
		}
		public static bool JoinedChannel(string name, User u)
		{
			return channels.Any(a => a.name == name && a.playerlist.Contains(u));
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
		public static Channel[] GetListJoinedChannel(User u)
		{
			return channels.Where(a => a.playerlist.Contains(u)).ToArray();
		}
		public static void RevokeChannel(string name, User u)
		{
			u.AddQueue(new BanchoPacket(PacketType.ServerChatChannelRevoked, new BanchoString(name)));
		}
		public static void SendMessage(string name, User u, string message)
		{
			Channel channel = GetChannel(name);
			if (channel == null) return;
			if (!JoinedChannel(name, u))
			{
				RevokeChannel(name, u);
				return;
			};
			foreach (User a in channel.playerlist)
				a.AddQueue(new BanchoPacket(PacketType.ServerChatMessage, new BanchoChatMessage(u.username, message, channel.name, u.userid)));
		}
		public static void SendMessageSecret(User input, User output, string message)
		{
			if (output == null) return;
				output.AddQueue(new BanchoPacket(PacketType.ServerChatMessage, new BanchoChatMessage(input.username, message, output.username, input.userid)));
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
		public List<User> playerlist = new List<User>();
	}
}
