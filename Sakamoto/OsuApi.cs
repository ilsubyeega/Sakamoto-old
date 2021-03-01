using Newtonsoft.Json;
using Sakamoto.Api;
using Sakamoto.Api.Legacy;
using Sakamoto.Helper.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sakamoto
{
	public static class OsuApi
	{
		public static string API_ROOT = @"https://osu.ppy.sh/api/v2";
		private static DateTime expiresAt;
		private static string accessToken = null;

		public static async Task<string> TryReqeust(string uri)
		{
			if (DateTime.Now.Ticks > expiresAt.Ticks || accessToken == null)
				await Refresh();
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
				var rq = await client.GetAsync(uri);
				return await rq.Content.ReadAsStringAsync();
			}
		}
		public static async Task<JsonBeatmap> FetchBeatmap(int beatmap_id)
		{
			if (DateTime.Now.Ticks > expiresAt.Ticks || accessToken == null)
				await Refresh();
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
				var rq = await client.GetAsync($"{API_ROOT}/beatmaps/lookup?id={beatmap_id}");
				if (rq.StatusCode != System.Net.HttpStatusCode.OK) return null;
				var value = await rq.Content.ReadAsStringAsync();
				return JsonConvert.DeserializeObject<JsonBeatmap>(value);
			}
		}
		public static async Task<JsonLegacyBeatmap> FetchLegacyBeatmap(int beatmap_id)
		{
			var legacyKey = ConfigParser.Config.Osu.ApiV1Key;
			if (legacyKey == null || legacyKey.Length == 0) throw new Exception("legacyKey should be not null.");
			using (var client = new HttpClient())
			{
				var value = await client.GetStringAsync($"https://osu.ppy.sh/api/get_beatmaps?k={legacyKey}&b={beatmap_id}");
				var rs = JsonConvert.DeserializeObject<JsonLegacyBeatmap[]>(value);
				if (rs.Length == 0) return null;
				if (rs.Length > 1) throw new Exception("beatmap id should be not returned twice");
				return rs[0];
			}
		}
		private static async Task Refresh()
		{
			try
			{
				using (var client = new HttpClient())
				{
					var osuconfig = ConfigParser.Config.Osu;
					var form = new MultipartFormDataContent();
					form.Add(new StringContent(osuconfig.ApiV2ClientId), "client_id");
					form.Add(new StringContent(osuconfig.ApiV2ClientSecret), "client_secret");
					form.Add(new StringContent("client_credentials"), "grant_type");
					form.Add(new StringContent("public"), "scope");

					var response = await client.PostAsync(@"https://osu.ppy.sh/oauth/token", form);
					if (!response.IsSuccessStatusCode)
						throw new Exception($"Refreshing new osu!api v2 token is failed; The status code was {response.StatusCode}");
					var val = await response.Content.ReadAsStringAsync();
					Console.WriteLine(val);
					var responsedynamic = JsonConvert.DeserializeObject<dynamic>(val);
					expiresAt = DateTime.Now.AddSeconds((int)responsedynamic.expires_in-3600);
					accessToken = (string)responsedynamic.access_token;
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}
	}
}
