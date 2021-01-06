using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sakamoto.Database;
using Sakamoto.Database.Models;
using Sakamoto.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sakamoto.Controllers
{
	[Route("")]
	[ApiController]
	public class RegisterController : ControllerBase
	{
		private readonly MariaDBContext _dbcontext;
		public RegisterController(MariaDBContext mariaDBContext) { _dbcontext = mariaDBContext; } 

		// POST /users
		[HttpPost("users")]
		[AllowAnonymous]
		public async Task<IActionResult> Register()
		{
			// todo "User registration is currently disabled"
			// todo Check Ip Ban
			// todo Check User-Agent
			var data = Request.Form.ToDictionary(x => x.Key, x => x.Value.ToString());
			var result = new
			{
				form_error = new
				{
					user = new Dictionary<string, List<string>>()
				}
			};
			string username, user_email, password;

			if (!data.TryGetValue("user[username]", out username)) CreateError("username", "The requested username is not found.", result.form_error.user);
			if (!data.TryGetValue("user[user_email]", out user_email)) CreateError("user_email", "The requested email is not found.", result.form_error.user);
			if (!data.TryGetValue("user[password]", out password)) CreateError("password", "The requested password is not found.", result.form_error.user);


			if (result.form_error.user.Count > 0
				|| !ValidateUsername(username, result.form_error.user)
				|| !ValidateEmail(user_email, result.form_error.user)
				|| !ValidatePassword(username, password, result.form_error.user))
			{
				return StatusCode(422, result);
			}
			try
			{
				var currentdate = DateTimeOffset.Now.ToUnixTimeSeconds();
				var ip = Request.HttpContext.Connection.RemoteIpAddress.ToString();
				var dbuser = new DBUser
				{
					UserName = username,
					Email = user_email,
					Password = CryptoUtil.EncodeScrypt(CryptoUtil.GetMD5Hash(password)),
					LastIP = ip,
					LastVisit = currentdate,
					RegisterationDate = currentdate,
					Language = 0,
					Country = GeoUtil.GetByIP(ip),
				};
				_dbcontext.Users.Add(dbuser);
				await _dbcontext.SaveChangesAsync();
			} catch (Exception e)
			{
				return StatusCode(422, new { error = "this is server side error, its error is logged and developer will investiage this issue." });
			}
			return StatusCode(200, new { result = "ok" });
		}

		private bool ValidateUsername(string name, Dictionary<string, List<string>> error)
		{
			if (name.StartsWith(" ") || name.EndsWith(" "))
				return CreateError("username", "Username can't start or end with spaces!", error);
			if (name.Length < 3)
				return CreateError("username", "The requested username is too short.", error);
			if (name.Length > 15)
				return CreateError("username", "The requested username is too long.", error);
			if (name.Contains("  ") || !Regex.IsMatch(name, @"^[a-zA-Z0-9_\[\] ]+$"))
				return CreateError("username", "The requested username contains invalid characters.", error);
			if (name.Contains("_") && name.Contains(" "))
				return CreateError("username", "Please use either underscores or spaces, not both!", error);
			// todo: not allowed names
			// todo: check used username
			var b = _dbcontext.Users.FirstOrDefault(a => a.UserName.ToLower() == name.ToLower());
			if (b != null)
				return CreateError("username", "That user is already exists.", error);

			return true;
		}
		private bool ValidatePassword(string name, string password, Dictionary<string, List<string>> error)
		{
			if (password.ToLower().Contains(name.ToLower())) 
				return CreateError("password", "Password may not contain username.", error);
			if (password.Length < 8) 
				return CreateError("password", "New password is too short.", error);
			// todo check weak password
			return true;
		}

		private bool ValidateEmail(string email, Dictionary<string, List<string>> error)
		{
			try
			{
				var addr = new System.Net.Mail.MailAddress(email);
				if (addr.Address != email)
					return CreateError("user_email", "Doesn't seem to be a valid email address.", error);
				if (!email.Contains("."))
					return CreateError("user_email", "Doesn't seem to be a valid email address.", error); // wher is dot
			}
			catch
			{
				return CreateError("user_email", "Doesn't seem to be a valid email address.", error);
			}
			// check banned emails for alt emails etc
			var b = _dbcontext.Users.FirstOrDefault(a => a.Email.ToLower() == email.ToLower());
			if (b != null)
				return CreateError("user_email", "That email is already used.", error);

			return true;
		}

		private bool CreateError(string type, string value, Dictionary<string, List<string>> error)
		{
			if (!error.ContainsKey(type)) error.Add(type, new List<string>());
			var list = new List<string>();
			error.TryGetValue(type, out list);
			list.Add(value);
			return false;
		}
	}
}
