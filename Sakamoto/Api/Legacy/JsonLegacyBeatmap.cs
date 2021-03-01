using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Api.Legacy
{
	public class JsonLegacyBeatmap
	{
		[JsonProperty("file_md5")]
		public string Checksum { get; set; }
	}
}
