using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Util
{
	public static class TimeUtil
	{
		public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
		{
			System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
			dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
			return dtDateTime;
		}
	}
}
