using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Sakamoto.Util
{
	public static class ReflectionUtil
	{
		public static void SetField(this object obj, string key, object value)
		{
			var mfield = obj.GetType().GetField(key, BindingFlags.NonPublic | BindingFlags.Instance);
			mfield.SetValue(obj, value);
		}
	}
}
