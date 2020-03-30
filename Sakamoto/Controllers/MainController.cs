using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace Sakamoto.Controllers
{
	[Route("")]
	public class MainController : Controller
	{
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			return Ok("Sakamoto (Bancho)\nosu!bancho reversing project");
		}
	}
}
