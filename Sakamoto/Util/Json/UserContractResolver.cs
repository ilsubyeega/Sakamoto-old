using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Util.Json
{
	public class UserContractResolver : DefaultContractResolver
	{
		protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
		{
			// Let the base class create all the JsonProperties 
			// using the short names
			IList<JsonProperty> list = base.CreateProperties(type, memberSerialization);
			List<JsonProperty> newlist = new List<JsonProperty>();
			foreach (JsonProperty prop in list)
			{
				if (prop.PropertyName.ToLower() == "user" || (prop.PropertyName.ToLower().Contains("rank") && prop.PropertyName.ToLower().Contains("history")))
					continue;
				if (prop.HasMemberAttribute && prop.Readable && prop.Writable)
					newlist.Add(prop);
			}
			var rank = list.FirstOrDefault(a => a.PropertyName.ToLower() == "rank_history");
			if (rank != null) newlist.Add(rank);
			
			return newlist;
		}
	}
}
