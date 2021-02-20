using System.Security.Cryptography;
using System.Text;

namespace Sakamoto.Util
{
	public static class CryptoUtil
	{
		private static Scrypt.ScryptEncoder _scryptEncoder = new Scrypt.ScryptEncoder();
		public static string GetMD5Hash(string input)
		{
			using (MD5 md5Hash = MD5.Create())
			{
				var bytearr = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
				var sBuilder = new StringBuilder();
				for (int i = 0; i < bytearr.Length; i++)
				{
					sBuilder.Append(bytearr[i].ToString("x2"));
				}
				return sBuilder.ToString();
			}
		}
		public static string EncodeScrypt(string input) => _scryptEncoder.Encode(input);
		public static bool CheckScrypt(string input, string hashed) => _scryptEncoder.Compare(input, hashed);
	}
}
