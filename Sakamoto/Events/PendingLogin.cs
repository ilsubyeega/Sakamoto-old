using HOPEless.Bancho;
using HOPEless.Bancho.Objects;
using osu.Shared.Serialization;
using Sakamoto.Cache;
using Sakamoto.Manager;
using Sakamoto.Objects;
using Sakamoto.Packet.Objects.Args;
using Sakamoto.Util;
using System;
using System.IO;

namespace Sakamoto.Events
{
	/// <summary>
	/// This class handles pending login, which is client is connecting to sakamoto without osu-token.
	/// </summary>
	public static class PendingLogin
	{
		public static void Handle(MemoryStream st, SerializationWriter writer, out string token)
		{
			token = null;
			st.Position = 0;

			PendingLoginArg loginarg = new PendingLoginArg(new StreamReader(st));
			if (loginarg.isValid)
			{
				token = TokenGenerator.Generate();
				int userid = 1; // test

				Console.WriteLine(loginarg.username + " joined with id " + userid);

				User user = new User
				{
					userid = userid,
					username = loginarg.username,
					username_safe = loginarg.username.ToLower(),
					chotoken = token,
					pp = new UserGame(),
					block_nonfriend = loginarg.block_non_friend_dms,
					timezone = loginarg.timezone,
					countryid = 0
				};
				UserCache.Add(user);

				user.AddQueue(new BanchoPacket(PacketType.ServerBanchoVersion, new BanchoInt(19)));
				user.AddQueue(new BanchoPacket(PacketType.ServerNotification, new BanchoString($"Welcome to Sakamoto\nBuild Date: {Common.build_date:yyyy-MM-dd HH:mm}")));

				user.AddQueue(new BanchoPacket(PacketType.ServerLoginReply, new BanchoInt(user.userid)));
				user.AddQueue(new BanchoPacket(PacketType.ServerUserPresence, user.ToPresence()));
				user.AddQueue(new BanchoPacket(PacketType.ServerUserData, user.ToUserData()));

				foreach (Channel channel in Manager.ChatManager.GetAutoJoinChannel(true))
				{
					user.AddQueue(new BanchoPacket(PacketType.ServerChatChannelAvailableAutojoin,
						new BanchoChatChannel(channel.name, channel.description, channel.GetUserCount())
						));
					user.AddQueue(new BanchoPacket(PacketType.ServerChatChannelJoinSuccess, new BanchoString(channel.name)));
				}
				user.AddQueue(new BanchoPacket(PacketType.ServerChatMessage, new BanchoChatMessage()
				{
					Channel = "#general",
					Message = "Welcome to Sakamoto!",
					Sender = "Sakamoto",
					SenderId = 2
				}));
				foreach (Channel channel in Manager.ChatManager.GetAutoJoinChannel(false))
					user.AddQueue(new BanchoPacket(PacketType.ServerChatChannelAvailable,
						new BanchoChatChannel(channel.name, channel.description, channel.GetUserCount())
						));
				UserUtil.SendAllUser(user);
				PacketUtil.WriteToStream(user.queue, writer);
				user.ClearQueue();

			}
		}
	}
}
