using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Sakamoto.Packet.Objects;
using Sakamoto.Packet.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
			if (Request.Headers["User-Agent"].ToString() == "osu!")
			{
				MemoryStream st = new MemoryStream();
				await Request.Body.CopyToAsync(st);
				st.Position = 0;
				if (RawPacketParser.isValid(st))
				{
					List<RawPacket> parse = RawPacketParser.Parse(st);
					for (int a = 0; a < parse.Count; a++)
					{
						Console.WriteLine(parse[a].ToString());
					}
					Console.WriteLine(parse.Count);
				}
				else
				{
					Console.WriteLine("The packet is invalid");
				}

			}
			return StatusCode(200, "Sakamoto (Bannedcho)\nosu!bancho reversing project"); //todo handle
		}
	}
}
