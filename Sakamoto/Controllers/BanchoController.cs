using HOPEless.Bancho;
using HOPEless.Bancho.Objects;
using HOPEless.osu;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using osu.Shared;
using osu.Shared.Serialization;
using Sakamoto.Packet.Objects;
using Sakamoto.Packet.Objects.Args;
using Sakamoto.Packet.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Sakamoto.Controllers
{
	[Route("/")]
	public class BanchoController : Controller
	{

		[HttpPost]
		[Host("c.ppy.sh", "ce.ppy.sh", "c1.ppy.sh", "c2.ppy.sh", "c3.ppy.sh", "c4.ppy.sh", "c5.ppy.sh", "c6.ppy.sh", "c7.ppy.sh", "c8.ppy.sh", "c9.ppy.sh",
			"c.osu.leu.kr", "ce.osu.leu.kr", "c1.osu.leu.kr", "c2.osu.leu.kr", "c3.osu.leu.kr", "c4.osu.leu.kr", "c5.osu.leu.kr", "c6.osu.leu.kr", "c7.osu.leu.kr", "c8.osu.leu.kr", "c9.osu.leu.kr")]
		public async Task<IActionResult> Post()
		{
			Response.Headers["cho-server"] = "Sakamoto (https://github.com/ilsubyeega/Sakamoto)";
			if (Request.Headers["User-Agent"].ToString() == "osu!")
			{
				MemoryStream ms = new MemoryStream();
				SerializationWriter writer = new SerializationWriter(ms);
				if (String.IsNullOrEmpty(Request.Headers["osu-token"]))
				{
					MemoryStream st = new MemoryStream();
					await Request.Body.CopyToAsync(st);
					st.Position = 0;

					PendingLoginArg loginarg = new PendingLoginArg(new StreamReader(st));
					if (loginarg.isValid)
					{
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
						new BanchoPacket(PacketType.ServerNotification, new BanchoString("ㅎㅇ")).WriteToStream(writer);

						ms.Position = 0;
						Response.Headers["cho-protocol"] = "19";
						Response.Headers["cho-token"] = "3521b0b8-4d7a-418e-aaf7-d853c4e7fake";
						List<RawPacket> parse = RawPacketParser.Parse(ms);
						for (int a = 0; a < parse.Count; a++)
						{
							Console.WriteLine(parse[a].ToString());
						}
						ms.Position = 0;
						return base.File(ms, "application/octet-stream");
					}
					else
					{
						Console.WriteLine("invalid Login");
					}
				}
				else
				{
					try
					{
						MemoryStream st = new MemoryStream();
						await Request.Body.CopyToAsync(st);
						st.Position = 0;
						List<BanchoPacket> list = PacketParser.Parse(st);
						for (int a = 0; a < list.Count; a++)
						{
							Console.WriteLine(list[a].ToString());
						}
					}
					catch
					{
						Console.WriteLine("The packet is invalid");
					}
				}
			}
			return StatusCode(200, "Sakamoto (Bannedcho)\nosu!bancho reversing project"); //todo handle
		}
	}
}
