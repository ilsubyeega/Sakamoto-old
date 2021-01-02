using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
