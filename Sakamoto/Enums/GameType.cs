using osu.Shared;

namespace Sakamoto.Enums
{
	public enum GameType
	{
		Osu,
		OsuTaiko,
		OsuCatch,
		OsuMania,
		OsuRelax,
		OsuTaikoRelax,
		OsuCatchRelax,
		OsuAutoPilot
	}
	public static class GameTypeUtil
	{
		public static GameMode getShared(GameType type)
		{
			if (type == GameType.OsuAutoPilot) return GameMode.Standard;
			if ((byte)type > 3)
				return (GameMode)((byte)type - 3);
			return (GameMode)type;
		}
	}
}
