using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Helper.Config
{
	public class Config
	{
		[JsonProperty("database")]
		public DatabaseConfig Database = new DatabaseConfig();
		[JsonProperty("osu")]
		public OsuConfig Osu = new OsuConfig();
		[JsonProperty("log")]
		public LogConfig Log = new LogConfig();
		[JsonProperty("directory")]
		public DirectoryConfig Directory = new DirectoryConfig();
		[JsonProperty("direct")]
		public DirectConfig Direct = new DirectConfig();
	}
	public class OsuConfig
	{
		[JsonProperty("apiv1_key")]
		public string ApiV1Key = "";
		[JsonProperty("apiv2_client_id")]
		public string ApiV2ClientId = "";
		[JsonProperty("apiv2_client_secret")]
		public string ApiV2ClientSecret = "";
	}
	public class LogConfig
	{
		[JsonProperty("enable_logging")]
		public bool EnableLogging = false;
		[JsonProperty("discord_webhook_url")]
		public string DiscordWebhookUrl = "";
	}
	public class DatabaseConfig
	{
		[JsonProperty("url")]
		public string Url = @"server=localhost;port=3306;database=keesu;uid=dev;password=dev";
	}
	public class DirectConfig
	{
		[JsonProperty("beatconnect_key")]
		public string BeatconnectKey = "";
	}
	public class DirectoryConfig
	{
		[JsonProperty("beatmap")]
		public string Beatmap = "";
		[JsonProperty("replay")]
		public string Replay = "";
	}
}
