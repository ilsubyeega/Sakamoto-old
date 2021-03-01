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
			if (from == null) return null;
			string q2 = "";
			foreach (var q3 in from.Split(" ", StringSplitOptions.RemoveEmptyEntries))
				if (q3.Length > 1 || String.Join("", q3.Split(" ")).Length > 1) q2 += " *" + String.Join("", q3.Split(spliitable, StringSplitOptions.TrimEntries));
			return $"*{q2}*";
		}
	}
}
