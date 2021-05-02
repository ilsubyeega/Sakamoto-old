using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sakamoto.Api;
using Sakamoto.Api.Chat;
using Sakamoto.Database;
using Sakamoto.Database.Models.Chat;
using Sakamoto.Enums.Chat;
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
	public class ChatController : SakamotoController
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
		[HttpPost("chat/channels/{channel_id}/messages")]
		public async Task<IActionResult> SendMessage(int channel_id, [FromForm] string message, [FromForm] bool is_action = false)
		{
			if (message == null) return StatusCode(402, "Message cannot be null.");

			var channel = await _dbcontext.Channels.FirstOrDefaultAsync(a => a.ChannelId == channel_id);
			if (channel == null) return StatusCode(404, "Channel not found");

			var dbMessage = new DBMessage
			{
				UserId = _user.Id,
				ChannelId = channel.ChannelId,
				Content = message,
				IsAction = is_action,
				Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds()
			};
			_dbcontext.Messages.Add(dbMessage);

			// If channel is PM, check target id is not registered at user channels and then add it.
			if (channel.Type == (int)ChannelType.PM)
			{
				var pmchannel = _dbcontext.PMChannels.FirstOrDefault(a => a.ChannelId == channel.ChannelId);
				var targetId = (_user.Id != pmchannel.UserId1) ? pmchannel.UserId1 : pmchannel.UserId2;
				var targetUserChannel = _dbcontext.UserChannels.Any(a => a.ChannelId == channel.ChannelId && a.UserId == targetId);
				if (targetUserChannel == null)
				_dbcontext.UserChannels.Add(new DBUserChannel
				{
					UserId = targetId,
					ChannelId = channel.ChannelId,
					LastReadId = dbMessage.MessageId-1
				});
			}

			await _dbcontext.SaveChangesAsync();

			return StatusCode(200, dbMessage.ToJsonMessage().IncludeSender(_user));
		}
		[HttpPost("chat/new")]
		public async Task<IActionResult> SendMessagePrivate([FromForm] int targetId, [FromForm] string message, [FromForm] bool is_action = false)
		{
			if (message == null) return StatusCode(402, "Message cannot be null.");
			var qq = await _dbcontext.PMChannels.FirstOrDefaultAsync(a => (a.UserId1 == targetId && a.UserId2 == _user.Id) 
			|| (a.UserId1 == _user.Id && a.UserId2 == targetId));
			if (qq == null) return StatusCode(404, "channel not found. should create this");
			var channel = await _dbcontext.Channels.FirstOrDefaultAsync(a => a.ChannelId == qq.ChannelId);
			if (channel == null) return StatusCode(404, "Channel not found");
			var dbMessage = new DBMessage
			{
				UserId = _user.Id,
				ChannelId = channel.ChannelId,
				Content = message,
				IsAction = is_action,
				Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds()
			};
			_dbcontext.Messages.Add(dbMessage);

			await _dbcontext.SaveChangesAsync();

			var uc = _dbcontext.UserChannels.FirstOrDefault(a => a.UserId == targetId && a.ChannelId == channel.ChannelId);
			if (uc == null)
			{
				_dbcontext.UserChannels.Add(new DBUserChannel
				{
					UserId = targetId,
					ChannelId = channel.ChannelId,
					LastReadId = dbMessage.MessageId
				});
				await _dbcontext.SaveChangesAsync();
			}
			return StatusCode(200, dbMessage.ToJsonMessage().IncludeSender(_user));
		}
		[HttpGet("chat/updates")]
		public async Task<IActionResult> GetUpdates(int since, int channel_id = -1, int limit = 50)
		{
			var userid = _user.Id;

			var userchannels = _dbcontext.UserChannels.Where(a => a.UserId == userid);
			if (userchannels == null) return StatusCode(200, new UpdateResponse().SerializeObject());

			

			var channels = _dbcontext.Channels.Where(a => userchannels.Any(b => b.UserId == userid && b.ChannelId == a.ChannelId));
			var messages = _dbcontext.Messages
				.Where(a => a.MessageId > since && userchannels.Any(b => b.UserId == userid && b.ChannelId == a.ChannelId))
				.OrderByDescending(a => a.MessageId)
				.Take(100)
				.OrderBy(a => a.MessageId);
			var jsonpresences = new List<JsonChannel>();
			var jsonmessage = new List<JsonMessage>();

			foreach (var a in channels)
			{
				var jsc = a.ToJsonChannel();
				if (jsc.Type == ChannelType.PM)
				{
					var pm = _dbcontext.PMChannels.FirstOrDefault(v => v.ChannelId == a.ChannelId);
					if (pm == null) continue;
					jsc.IncludeUsers(new int[] { pm.UserId1, pm.UserId2 });
					jsc.Name = (userid != pm.UserId1) ? 
						_user.UserName : _dbcontext.Users.FirstOrDefault(a => a.Id == pm.UserId2)?.UserName ?? jsc.Name;
				}
				jsonpresences.Add(jsc);
			}
				

			foreach (var a in messages)
			{
				var user = _dbcontext.Users.FirstOrDefault(b => b.Id == a.UserId);
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
