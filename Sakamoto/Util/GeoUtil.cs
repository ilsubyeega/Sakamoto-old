using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Sakamoto.Util
{
	public static class GeoUtil
	{
		public static string GetByIP(string ip)
		{
			try
			{
				string info = new WebClient().DownloadString("http://ip.zxq.co/" + ip);
				dynamic data = JsonConvert.DeserializeObject<dynamic>(info);
				var country = (string)data.country;
				return country.Length < 2 ? "??" : country;
			} catch
			{
				return "??";
			}
		}
	}
}
