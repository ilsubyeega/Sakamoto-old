using HOPEless.Bancho;
using HOPEless.Bancho.Objects;
using HOPEless.osu;
using osu.Shared.Serialization;

namespace Sakamoto.Events.Packet
{
	public static class ClientUserStatus
	{
		public static void Handle(BanchoPacket packet, SerializationWriter writer)
		{
			BanchoUserStatus status = new BanchoUserStatus(packet.Data);
			new BanchoPacket(PacketType.ServerUserData, new BanchoUserData()
			{
				UserId = 1,
				Status = new BanchoUserStatus()
				{
					Action = BanchoAction.Idle,
					ActionText = "몰라",
					BeatmapChecksum = "aaaaaaaaaaa",
					CurrentMods = status.CurrentMods,
					PlayMode = status.PlayMode,
					BeatmapId = 1
				},
				RankedScore = 100,
				Accuracy = 100,
				Playcount = 0,
				TotalScore = 1000,
				Rank = 1,
				Performance = 10000
			}).WriteToStream(writer);
		}
	}
}
