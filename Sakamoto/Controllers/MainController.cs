using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


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
