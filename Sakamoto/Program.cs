using System;

namespace Sakamoto
{
	class Program
	{
		static void Main(string[] args)
		{
			Server.CServer.Init();
			ReadLine();
			Console.WriteLine("Exited");
		}
		static void ReadLine()
		{
			Console.ReadLine();
			ReadLine();
		}
	}
}
