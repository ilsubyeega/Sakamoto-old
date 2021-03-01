using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Enums
{
	[Flags]
	public enum UserPermission
	{
		Default = 0,

		SystemAdmin = 1 << 6,
		Developer = 1 << 7
	}
}
