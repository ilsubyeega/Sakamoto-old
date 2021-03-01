using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sakamoto.Api;
using Sakamoto.Api.Chat;
using Sakamoto.Database;
using Sakamoto.Database.Models.Chat;
using Sakamoto.Transformer.Chat;
using Sakamoto.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Controllers.Chat
{
	[ApiController]
	[Route("api/v2/")]
	[Authorize]
	public class ChatController : ControllerBase
	{
		private readonly MariaDBContext _dbcontext;
		public ChatController(MariaDBContext mariaDBContext) { _dbcontext = mariaDBContext; }

		[HttpGet("chat/channels/{channel}/messages")]
		public async Task<IActionResult> GetChannelMessages(int channel, int limit = 50, int since = 0, int until = -1)
		{
			// todo: should check permission

			var q = _dbcontext.Messages.Where(a => a.ChannelId == channel && a.MessageId > since).OrderByDescending(a => a.MessageId).Take(50);
			var list = new List<JsonMessage>();
			foreach (var a in q)
			{
				var user = await _dbcontext.Users.FirstOrDefaultAsync(b => a.UserId == b.Id);
				list.Add(a.ToJsonMessage().IncludeSender(user));
			}
				
			return StatusCode(200, list);
		}
		[HttpPost("chat/channels/{channel}/messages")]
		public async Task<IActionResult> SendMessage(int channel, [FromForm] string message, [FromForm] bool is_action = false)
		{
			if (message == null) return StatusCode(402, "Message cannot be null.");
			var userid = (int)HttpContext.Items["userId"];
			var q = await _dbcontext.Channels.FirstOrDefaultAsync(a => a.ChannelId == channel);
			var qu = await _dbcontext.Users.FirstOrDefaultAsync(a => a.Id == userid);
			if (q == null) return StatusCode(404, "Channel not found");
			var dbmsg = new DBMessage
			{
				UserId = userid,
				ChannelId = q.ChannelId,
				Content = message,
				IsAction = is_action,
				Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds()
			};
			_dbcontext.Messages.Add(dbmsg);
			await _dbcontext.SaveChangesAsync();
			return StatusCode(200, dbmsg.ToJsonMessage().IncludeSender(qu));
		}

		[HttpGet("chat/updates")]
		public async Task<IActionResult> GetUpdates(int since, int channel_id = -1, int limit = 50)
		{
			var userid = (int)HttpContext.Items["userId"];

			var userchannels = _dbcontext.UserChannels.Where(a => a.UserId == userid);
			if (userchannels == null) return StatusCode(200, new UpdateResponse().SerializeObject());

			

			var channels = _dbcontext.Channels.Where(a => userchannels.Any(b => b.ChannelId == a.ChannelId));
			var messages = _dbcontext.Messages
				.Where(a => a.MessageId > since && userchannels.Any(b => b.ChannelId == a.ChannelId))
				.OrderByDescending(a => a.MessageId)
				.Take(100)
				.OrderBy(a => a.MessageId);
			var jsonpresences = new List<JsonChannel>();
			var jsonmessage = new List<JsonMessage>();

			foreach (var a in channels)
				jsonpresences.Add(a.ToJsonChannel());

			foreach (var a in messages)
			{
				var user = _dbcontext.Users.FirstOrDefault(a => a.Id == a.Id);
				var m = a.ToJsonMessage();
				if (user != null) m.IncludeSender(user);
				jsonmessage.Add(m);
			}
				

			return StatusCode(200, new UpdateResponse
			{
				presence = jsonpresences.ToArray(),
				messages = jsonmessage.ToArray()
			});
		}

		private class UpdateResponse
		{
			public JsonChannel[] presence = new JsonChannel[] { };
			public JsonMessage[] messages = new JsonMessage[] { };
		}
	}
}
