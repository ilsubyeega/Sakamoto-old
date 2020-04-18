using HOPEless.Bancho;
using HOPEless.Bancho.Objects;
using HOPEless.osu;
using osu.Shared;
using osu.Shared.Serialization;
using Sakamoto.Manager;
using Sakamoto.Packet.Objects.Args;
using System.IO;

namespace Sakamoto.Events
{
	public static class PendingLogin
	{
		public static void Handle(MemoryStream st, SerializationWriter writer, out string token)
		{
			token = null;
			st.Position = 0;

			PendingLoginArg loginarg = new PendingLoginArg(new StreamReader(st));
			if (loginarg.isValid)
			{
				token = "3521b0b8-4d7a-418e-aaf7-d853c4e7fake";
				new BanchoPacket(PacketType.ServerBanchoVersion, new BanchoInt(19)).WriteToStream(writer);
				new BanchoPacket(PacketType.ServerLoginReply, new BanchoInt(1)).WriteToStream(writer);
				new BanchoPacket(PacketType.ServerUserPresence, new BanchoUserPresence()
				{
					UserId = 1,
					UsesOsuClient = true,
					Timezone = 9,
					CountryCode = 0,
					Permissions = PlayerRank.Supporter,
					Longitude = 1.2f,
					Latitude = 1.2f,
					Rank = 1
				}).WriteToStream(writer);
				new BanchoPacket(PacketType.ServerUserData, new BanchoUserData()
				{
					UserId = 1,
					Status = new BanchoUserStatus()
					{
						Action = BanchoAction.Idle,
						ActionText = "몰라",
						BeatmapChecksum = "aaaaaaaaaaa",
						CurrentMods = Mods.Easy,
						PlayMode = GameMode.Standard,
						BeatmapId = 1
					},
					RankedScore = 100,
					Accuracy = 100,
					Playcount = 0,
					TotalScore = 1000,
					Rank = 1,
					Performance = 10000
				}).WriteToStream(writer);
				new BanchoPacket(PacketType.ServerNotification, new BanchoString("Welcome to Sakamoto")).WriteToStream(writer);
				foreach (Channel channel in Manager.ChatManager.GetAutoJoinChannel(true))
				{
					new BanchoPacket(PacketType.ServerChatChannelAvailableAutojoin,
						new BanchoChatChannel(channel.name, channel.description, channel.GetUserCount())
						).WriteToStream(writer);
					new BanchoPacket(PacketType.ServerChatChannelJoinSuccess, new BanchoString(channel.name)).WriteToStream(writer);

				}
				new BanchoPacket(PacketType.ServerChatMessage, new BanchoChatMessage()
				{
					Channel = "#general",
					Message = "Poggers",
					Sender = "Sakamoto",
					SenderId = 2
				}).WriteToStream(writer);
				foreach (Channel channel in Manager.ChatManager.GetAutoJoinChannel(false))
					new BanchoPacket(PacketType.ServerChatChannelAvailable,
						new BanchoChatChannel(channel.name, channel.description, channel.GetUserCount())
						).WriteToStream(writer);


			}
		}
	}
}
