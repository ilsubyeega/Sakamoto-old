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
				try
				{
					return JsonConvert.DeserializeObject<BeatconnectSetJson>(data.Content.ReadAsStringAsync().Result);
				} catch (Exception e)
				{
					if (data.StatusCode == HttpStatusCode.NotFound)
						return null;
					throw e;
				}
				
			}
		}
	}
	public class BeatconnectSetJson
	{
		[JsonProperty("unique_id")]
		public string UUID;
		[JsonProperty("last_updated")]
		public string LastUpdated;
		[JsonProperty("ranked_date")]
		public string RankedDate; // nullable
	}
}
