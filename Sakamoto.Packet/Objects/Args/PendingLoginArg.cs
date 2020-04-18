using System;
using System.IO;

namespace Sakamoto.Packet.Objects.Args
{
	public class PendingLoginArg
	{
		public bool isValid = true;

		public string username;
		public string password_md5;
		public string version;
		public sbyte timezone;
		public bool display_location;
		public string hash;
		public bool block_non_friend_dms;

		public PendingLoginArg(StreamReader reader)
		{
			this.username = reader.ReadLine();
			this.password_md5 = reader.ReadLine();
			string data = reader.ReadLine();
			if (String.IsNullOrEmpty(data))
			{
				this.isValid = false;
				Console.WriteLine(username + password_md5 + "data is empty");
			}
			else
			{
				string[] _tmp = data.Split("|");
				if (_tmp.Length == 5 || _tmp == null)
				{
					this.version = _tmp[0];
					this.timezone = sbyte.Parse(_tmp[1]);
					this.display_location = (_tmp[2] == "1");
					this.hash = _tmp[3];
					this.block_non_friend_dms = (_tmp[4] == "1");
					this.CheckValid();
				}
				else
				{
					this.isValid = false;
				}
			}
		}

		public PendingLoginArg(string username, string password_md5, string version, sbyte timezone, bool display_location, string hash, bool block_non_friend_dms)
		{
			this.username = username;
			this.password_md5 = password_md5;
			this.version = version;
			this.timezone = timezone;
			this.display_location = display_location;
			this.hash = hash;
			this.block_non_friend_dms = block_non_friend_dms;
			this.CheckValid();
		}

		private void CheckValid()
		{
			if (isValid == true)
				isValid = !(String.IsNullOrEmpty(username) &&
					String.IsNullOrEmpty(password_md5) &&
					String.IsNullOrEmpty(version) &&
					String.IsNullOrEmpty(hash));
		}
	}
}
