using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Api
{
	public class JsonMod
	{
		[JsonProperty("acronym")]
		public string Acronym { get; set; }
		[JsonProperty("settings")]
		public Dictionary<string, object> Settings { get; set; } = new Dictionary<string, object>();
	}
}
