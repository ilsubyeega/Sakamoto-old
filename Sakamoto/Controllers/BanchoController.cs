using HOPEless.Bancho;
using HOPEless.Bancho.Objects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using osu.Shared.Serialization;
using Sakamoto.Cache;
using Sakamoto.Events;
using Sakamoto.Objects;
using Sakamoto.Util;
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
			Response.Headers["cho-protocol"] = "19";
			if (Request.Headers["User-Agent"].ToString() == "osu!")
			{
				MemoryStream ms = new MemoryStream();
				SerializationWriter writer = new SerializationWriter(ms);
				if (String.IsNullOrEmpty(Request.Headers["osu-token"]))
				{
					// if client requests without tokens, it means client probably going to login.
					MemoryStream st = new MemoryStream();
					await Request.Body.CopyToAsync(st);
					Events.PendingLogin.Handle(st, writer, out string token);
					if (token != null)
						Response.Headers["cho-token"] = token;
					ms.Position = 0;
					//PacketParser.Debug(ms);
					ms.Position = 0;
				}
				else
				{
					try
					{
						User u = OnlineUserCache.GetUserByToken(Request.Headers["osu-token"]);
						if (u != null)
						{
							// Read for debuging
							MemoryStream st = new MemoryStream();
							await Request.Body.CopyToAsync(st);
							st.Position = 0;
							u.lasttimestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
							List<BanchoPacket> list = PacketParser.Parse(st);
							foreach (BanchoPacket packet in list)
							{
								PacketEventHandler.Handle(packet, u);
							}
							/*Console.WriteLine("");
							foreach (BanchoPacket packet in u.queue) { 
								Console.WriteLine(packet.ToString());
							}*/
							// Write
							PacketUtil.WriteToStream(u.queue, writer);
							u.ClearQueue();
						}
						else
						{
							// We cannot find the tokens in memory, so just reconnect it.
							new BanchoPacket(PacketType.ServerRestart, new BanchoInt(0)).WriteToStream(writer);
						}


						/*
						new BanchoPacket(PacketType.ServerChatMessage, new BanchoChatMessage()
						{
							Channel = "#general",
							Message = "Poggers",
							Sender = "Sakamoto",
							SenderId = 2
						}).WriteToStream(writer);*/


					}
					catch (Exception e)
					{

						Console.WriteLine("The packet is invalid\n" + e.StackTrace);
					}
				}
				ms.Position = 0;
				//PacketParser.Debug(ms);
				ms.Position = 0;
				return base.File(ms, "application/octet-stream");
			}
			return StatusCode(200, "Sakamoto (Bannedcho)\nosu!bancho reversing project");
		}

	}
}