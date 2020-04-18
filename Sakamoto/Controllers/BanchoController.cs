using HOPEless.Bancho;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using osu.Shared.Serialization;
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
				}
				else
				{
					try
					{
						// Read for debuging
						MemoryStream st = new MemoryStream();
						await Request.Body.CopyToAsync(st);
						st.Position = 0;
						List<BanchoPacket> list = PacketParser.Parse(st);
						for (int a = 0; a < list.Count; a++)
						{
							Console.WriteLine(list[a].ToString());
						}

						/*
						new BanchoPacket(PacketType.ServerChatMessage, new BanchoChatMessage()
						{
							Channel = "#general",
							Message = "Poggers",
							Sender = "Sakamoto",
							SenderId = 2
						}).WriteToStream(writer);*/

						// todo handle
					}
					catch
					{

						Console.WriteLine("The packet is invalid");
					}
				}
				ms.Position = 0;
				return base.File(ms, "application/octet-stream");
			}
			return StatusCode(200, "Sakamoto (Bannedcho)\nosu!bancho reversing project");
		}

	}
}