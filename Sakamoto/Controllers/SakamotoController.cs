using Microsoft.AspNetCore.Mvc;
using Sakamoto.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Controllers
{
	public class SakamotoController : ControllerBase
	{
		// This two property should exists when [Authorize] Attribute is set.
		private int? _userid 
		{
			get => (int?)HttpContext.Items["userId"];
		}
		private DBUser _user 
		{
			get => HttpContext.Items["user"] as DBUser;
		}

		private ActionResult SendError(int statuscode, string message)
			=> StatusCode(statuscode, new
			{
				error = message
			});
		private ActionResult SendException(int statuscode, Exception e, bool send)
		{
			var msg = new
			{
				error = "Error while performing the response.",
				exception = new
				{
					message = e.Message
				}
			};
			return StatusCode(statuscode);
		}

	}
}
