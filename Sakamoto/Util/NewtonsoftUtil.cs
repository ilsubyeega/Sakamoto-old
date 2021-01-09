using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Sakamoto.Util.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Util
{
	public static class NewtonsoftUtil
	{
		public static string SerializeObject(this object obj)
		{
			var serializerSettings = new JsonSerializerSettings();
			serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
			return JsonConvert.SerializeObject(obj, serializerSettings);
		}
		public static string SerializeUser(this osu.Game.Users.User obj)
		{
			var serializerSettings = new JsonSerializerSettings();
			serializerSettings.ContractResolver = new UserContractResolver();
			return JsonConvert.SerializeObject(obj, serializerSettings);
		}
	}

	
}
