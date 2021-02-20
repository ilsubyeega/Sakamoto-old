using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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
			return JsonConvert.SerializeObject(obj);
		}
	}

	
}
