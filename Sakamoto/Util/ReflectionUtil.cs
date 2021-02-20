using System.Reflection;

namespace Sakamoto.Util
{
	public static class ReflectionUtil
	{
		public static void SetField(this object obj, string key, object value)
		{
			var type = obj.GetType();
			var mfield = type.GetProperty(key, BindingFlags.NonPublic | BindingFlags.Instance);
			mfield.SetValue(obj, value);
		}
	}
}
