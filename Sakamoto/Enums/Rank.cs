using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Enums
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum Rank
	{
		A,
		B,
		C,
		D,
		S,
		SH,
		X,
		XH,
		F
	}
}
