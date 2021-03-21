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
			" ", "+", "-", ">", "<", "(", ")", "~", "*", "\\", "\"", "/"
		};

		public static string ToQuery(string from)
		{
			if (from == null || from.Length == 0) return null;

			var splitted = from.Split(" ", StringSplitOptions.RemoveEmptyEntries);

			if (splitted.Length == 1)
				return "*" + String.Join("", splitted[0].Split(spliitable, StringSplitOptions.TrimEntries)) + "*";

			string q2 = "";

			foreach (var q3 in splitted)
				if (q3.Length > 1 || String.Join("", q3.Split(" ")).Length > 1) q2 += " *" + String.Join("", q3.Split(spliitable, StringSplitOptions.TrimEntries));

			return $"*{q2}*";
		}
	}
}
