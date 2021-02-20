using System;
using System.IO;

namespace Sakamoto
{
	public static class Common
	{
		private static byte[] _key = null;
		public static byte[] GetKey()
		{
			if (_key == null)
			{
				var b64 = File.ReadAllText("public.key");
				_key = Convert.FromBase64String(b64);
			}
			return _key;
		}
	}
}
