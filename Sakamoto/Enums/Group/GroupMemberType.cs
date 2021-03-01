using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Enums.Group
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum GroupMemberType
	{
		Unknown = 0,
		Member = 1,
		Junior = 2,
		Senior = 3,
		Elder = 4,
		Staff = 5,
		Leader = 6
	}
}
