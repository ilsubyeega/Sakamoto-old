using System;
using System.IO;
using osu.Shared.Serialization;
namespace Sakamoto.Test
{
	class Program
	{
		// Add packet array below (Using fiddler Inspectors -> Response -> HexView -> Copy -> Copy as 0x##)
		static byte[] arrOutput = {  };
		static string a = "a";
		static void Main(string[] args)
		{
			Test(a);
			Console.WriteLine(a);
		}
		static void Test(string b)
		{
			b = "owo";
		}
		static void Parse()
		{
			var stream = new MemoryStream(arrOutput);
			SerializationReader r = new SerializationReader(stream);
			while (r.BaseStream.Position != r.BaseStream.Length)
			{
				Console.WriteLine("Type: " + (PacketType)r.ReadInt16());
				r.ReadByte();

				int length = r.ReadInt32();
				Console.WriteLine("Length: " + length);
				r.ReadBytes(length);
				//Console.WriteLine("Data: " + r.ReadBytes(length).ToString());
			}
		}
	}
}
