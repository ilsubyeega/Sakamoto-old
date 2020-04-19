using System;
using System.Linq;

namespace Sakamoto.Util
{

	public static class TokenGenerator
	{
		private static Random random = new Random();
		private static string chars = "abcdefghijklmnoqrstuvwxyz01234567890";
		public static string Generate()
		{
			return new string(Enumerable.Repeat(chars, 20).Select(s => s[random.Next(s.Length)]).ToArray());
		}

	}
}
