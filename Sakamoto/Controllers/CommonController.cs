using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Controllers
{
	[ApiController]
	public class CommonController : ControllerBase
	{
		[Route("/", Order = -99)] // in case of
		[Route("/api/v2", Order = -99)]
		public IActionResult V2Index()
		{
			return Ok("Sakamoto!");
		}
	}
}
