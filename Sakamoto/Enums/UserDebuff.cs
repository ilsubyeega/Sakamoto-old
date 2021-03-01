using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Enums
{
	[Flags]
	public enum UserDebuff
	{
		None = 0,
		NoCustomize = 1 << 0,

		Unranked = 1 << 1,

		Muted = 1 << 2,
		Banned = 1 << 3,

		AutoBan = 1 << 4
	}
}
