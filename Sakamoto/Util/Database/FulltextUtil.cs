using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Util.Database
{
	public static class FulltextUtil
	{

		// /[+\-><\(\)~*\\\" /]+/g
		private static string[] spliitable = new string[]
		{
			"+", "-", ">", "<", "(", ")", "~", "*", "\\", "\"", "/"
		};

		public static string ToQuery(string from)
		{
			if (from == null || from.Length == 0) return null;

			string q2 = String.Join("", from.Split(spliitable, StringSplitOptions.TrimEntries));
			return $"{q2}";
		}
	}
}
