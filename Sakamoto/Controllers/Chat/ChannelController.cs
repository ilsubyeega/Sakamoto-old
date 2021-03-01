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
	
	public class ChannelController : ControllerBase
	{
		private readonly MariaDBContext _dbcontext;
		public ChannelController(MariaDBContext mariaDBContext) { _dbcontext = mariaDBContext; }


		[HttpGet("chat/channels")]
		
		public IActionResult GetChannelList()
		{
			var channels = _dbcontext.Channels.Where(a => a.Type == (int)ChannelType.PUBLIC);
			var list = new List<JsonChannel>();
			foreach (var a in channels)
			{
				var json = a.ToJsonChannel();
				if (json.Type == ChannelType.PM)
				{
					var pm = _dbcontext.PMChannels.FirstOrDefault(v => v.ChannelId == a.ChannelId);
					if (pm == null) continue;
					json.IncludeUsers(new int[] { pm.UserId1, pm.UserId2 });
				}	
				list.Add(json);
			}
				
			return StatusCode(200, list);
		}
		[HttpPost("chat/channels")]
		public async Task<IActionResult> CreateChannel([FromForm] string type, [FromForm] int? target_id = null)
		{
			if (target_id == null) return StatusCode(422, "Targetid not provided.");

			var typeenum = (ChannelType?)Enum.Parse(typeof(ChannelType), type);
			if (typeenum == null) return StatusCode(422, "ChannelType not found.");
			if (typeenum != ChannelType.PM) return StatusCode(422, "Current channel type is not supported atm");

			var userid = (int)HttpContext.Items["userId"];

			var channel = await _dbcontext.PMChannels.FirstOrDefaultAsync(a => a.UserId1 == userid || a.UserId2 == userid);
			DBChannel chan;
			if (channel == null) // if pm channel not found, create.
			{
				var c = new DBChannel
				{
					Name = $"pm_{userid}_{target_id}",
					CreationTime = DateTimeOffset.Now.ToUnixTimeSeconds(),
					Type = (int)ChannelType.PM
				};
				_dbcontext.Channels.Add(c);
				await _dbcontext.SaveChangesAsync();
				var p = new DBChannelPM
				{
					ChannelId = c.ChannelId,
					UserId1 = userid,
					UserId2 = target_id.Value
				};
				_dbcontext.PMChannels.Add(p);
				await _dbcontext.SaveChangesAsync();
				chan = c;
				channel = p;
			}
			else
			{
				chan = await _dbcontext.Channels.FirstOrDefaultAsync(a => a.ChannelId == channel.ChannelId);
				if (chan == null) return StatusCode(404, "Channel not found (its a bug, report this to developer)");
			}
			var userchannel = await _dbcontext.UserChannels.FirstOrDefaultAsync(a => a.ChannelId == chan.ChannelId && a.UserId == userid);
			if (userchannel == null)
			{
				var uc = new DBUserChannel
				{
					UserId = userid,
					ChannelId = chan.ChannelId,
					LastReadId = 0
				};
				_dbcontext.UserChannels.Add(uc);
				await _dbcontext.SaveChangesAsync();
			}
			var messages = _dbcontext.Messages.Where(a => a.ChannelId == chan.ChannelId).OrderByDescending(a => a.Timestamp).Take(50);
			messages = messages.OrderBy(a => a.Timestamp);
			var jsonchannel = chan.ToJsonChannel();
			jsonchannel.SetLastRead(0);
			jsonchannel.IncludeMessages(messages.ToArray(), _dbcontext);
			jsonchannel.IncludeUsers(new int[] { userid, target_id.Value });
			return StatusCode(200, jsonchannel);
		}
		[HttpPut("chat/channels/{channelid}/users/{userid}")]
		public async Task<IActionResult> JoinChannel(int channelid, int userid)
		{
			var channel = await _dbcontext.Channels.FirstOrDefaultAsync(a => a.ChannelId == channelid);
			if (channel == null) return StatusCode(404, "Channel not found.");

			/*
			 * TODO: implement perm checking after making permissions.
			 * 
			if ((ChannelType)channel.Type != ChannelType.PUBLIC)
			{
				var user = await _dbcontext.Users.FirstOrDefaultAsync(a => (int)HttpContext.Items["userId"] == userid);
				
			}*/

			var jsonchannel = channel.ToJsonChannel();

			var userchannel = new DBUserChannel
			{
				UserId = (int)HttpContext.Items["userId"],
				ChannelId = channel.ChannelId,
				IsHidden = false,
				LastReadId = 0
			};
			_dbcontext.UserChannels.Add(userchannel);
			await _dbcontext.SaveChangesAsync();
			var rs = jsonchannel;
			Console.WriteLine(rs.SerializeObject());
			return StatusCode(200, rs);
		}
		[HttpDelete("chat/channels/{channelid}/users/{userid}")]
		public async Task<IActionResult> LeaveChannel(int channelid, int userid)
		{
			var userchannel = await _dbcontext.UserChannels.FirstOrDefaultAsync(a => a.ChannelId == channelid && a.UserId == (int)HttpContext.Items["userId"]);
			if (userchannel == null) return StatusCode(404, "Channel is not found or user didnt join the channel.");
			_dbcontext.UserChannels.Remove(userchannel);
			await _dbcontext.SaveChangesAsync();
			return StatusCode(200, "");
		}


	}
}
