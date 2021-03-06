using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Helper.Config
{
	public static class ConfigParser
	{
		public static Config Config = null;

		private static string EnvironmentKey = "SAKAMOTO_CONFIG_PATH";

		public static void Parse()
		{
			var path = Environment.GetEnvironmentVariable(EnvironmentKey);
			if (path == null) path = "/config.json";

			if (!File.Exists(path))
			{
				Console.WriteLine("Couldnt find config, so we created the config :3");
				Console.WriteLine($"Path of config is " + Path.GetFullPath(path));
				Console.WriteLine("Please edit config.json and restart the Sakamoto!");
				CreateConfig(path);
				Environment.Exit(1);
			} else
			{
				ParseConfig(path);
			}
		}
		public static void ParseConfig(string path)
		{
			var value = File.ReadAllText(path);
			var config = JsonConvert.DeserializeObject<Config>(value);
			File.WriteAllText(path, JsonConvert.SerializeObject(config, Formatting.Indented));
			Config = config;
		}
		public static void CreateConfig(string path)
		{
			Config = new Config();
			var str = JsonConvert.SerializeObject(Config, Formatting.Indented);
			File.WriteAllText(path, str);
		}
	}
}
