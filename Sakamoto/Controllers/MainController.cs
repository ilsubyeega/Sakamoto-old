﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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