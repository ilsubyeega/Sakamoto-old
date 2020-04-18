using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Objects
{
	public class DummyUser
	{
		public int id;
		public string username;
		public string username_safe;
		public bool is_restricted;
		public UserGame status = new UserGame();
	}
}
