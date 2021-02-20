using Newtonsoft.Json;

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
