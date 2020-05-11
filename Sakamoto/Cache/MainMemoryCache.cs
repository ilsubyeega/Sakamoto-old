using System.Runtime.Caching;

namespace Sakamoto.Cache
{
	public static class MainMemoryCache
	{
		public static MemoryCache Beatmap = new MemoryCache("beatmap");

	}
}
