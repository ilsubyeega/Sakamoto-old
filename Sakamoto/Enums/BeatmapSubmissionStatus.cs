using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Enums
{
	public enum BeatmapSubmissionStatus
	{
		Unknown = -2,
		NotSubmitted = -1,
		Pending = 0,
		Need_Update = 1,
		Ranked = 2,
		Approved = 3,
		Qualified = 4,
		Loved = 5
	}
}
