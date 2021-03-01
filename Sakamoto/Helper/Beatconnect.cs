using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sakamoto.Helper
{
	public static class Beatconnect
	{
		private static string key() => Config.ConfigParser.Config.Direct.BeatconnectKey;

		public static BeatconnectSetJson Fetch(int beatmapsetid)
		{
			using (var client = new HttpClient())
			{
				var data = client.GetAsync($"https://beatconnect.io/api/beatmap/{beatmapsetid}/?token={key()}").Result;
				return JsonConvert.DeserializeObject<BeatconnectSetJson>(data.Content.ReadAsStringAsync().Result);
			}
		}
	}
	public class BeatconnectSetJson
	{
		[JsonProperty("unique_id")]
		public string UUID;
	}
}
