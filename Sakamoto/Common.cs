using System;
using System.IO;
using System.Reflection;

namespace Sakamoto
{
	public static class Common
	{
		public static DateTime build_date = new FileInfo(Assembly.GetExecutingAssembly().Location).LastWriteTime;
	}
}
