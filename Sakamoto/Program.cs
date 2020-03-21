using System;

namespace Sakamoto
{
	class Program
	{
		static void Main(string[] args)
		{
			Server.CServer.Init();
			Console.ReadLine();
			Console.WriteLine("Exited");
		}
	}
}
