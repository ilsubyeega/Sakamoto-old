using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace Sakamoto.Controllers
{
	[Route("")]
	public class MainController : Controller
	{
		[HttpGet]
#pragma warning disable CS1998 // 이 비동기 메서드에는 'await' 연산자가 없으며 메서드가 동시에 실행됩니다.
		public async Task<IActionResult> Get()
#pragma warning restore CS1998 // 이 비동기 메서드에는 'await' 연산자가 없으며 메서드가 동시에 실행됩니다.
		{
			return Ok("Sakamoto (Bancho) \nosu!bancho reversing project");
		}
	}
}
