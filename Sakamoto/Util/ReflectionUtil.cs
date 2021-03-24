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

		public static void Copy(object original, object to)
		{
			foreach (var property in original.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
			{
				PropertyInfo propertyS = to.GetType().GetProperty(property.Name);

				if (!propertyS.CanWrite || !propertyS.CanRead) continue;

				MethodInfo mget = property.GetGetMethod(false);
				MethodInfo mset = property.GetSetMethod(false);

				if (mget == null || mset == null) continue; // shouldve be public.

				var _val = property.GetValue(original, null);
				propertyS.SetValue(to, _val, null);
			}
		}
	}
}
