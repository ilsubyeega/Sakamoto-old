using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Enums.Group
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum GroupType
	{
		Unknown = 0,
		Normal = 1, // could ordered by ranked
		Unranked = 2
	}
}
