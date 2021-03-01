using Sakamoto.Api;
using Sakamoto.Enums;
using System;

namespace Sakamoto.Util
{
	public static class GradeUtil
	{
		public static Rank CalculateOsu(int mods, int c50, int c100, int c300, int cMiss)
		{
			var mod = (Mods)mods;
			var totalHits = c50 + c100 + c300 + cMiss;

			if (c300 == totalHits || totalHits == 0)
				return (mod & (Mods.Hidden | Mods.Flashlight)) > 0 ? Rank.XH : Rank.X;

			var ratio300 = (float)c300 / totalHits;
			var ratio50 = (float)c50 / totalHits;

			if (ratio300 > 0.9 && ratio50 <= 0.01 && cMiss == 0)
				return (mod & (Mods.Hidden | Mods.Flashlight)) > 0 ? Rank.SH : Rank.S;

			if ((ratio300 > 0.8 && cMiss == 0) || ratio300 > 0.9) return Rank.A;
			if ((ratio300 > 0.7 && cMiss == 0) || ratio300 > 0.8) return Rank.B;
			if (ratio300 > 0.6) return Rank.C;

			Console.WriteLine($"{totalHits} {ratio300} {ratio50}");
			return Rank.D;
		}
		enum Mods
		{
			None = 0,
			NoFail = 1,
			Easy = 2,
			TouchDevice = 4,
			Hidden = 8,
			HardRock = 16,
			SuddenDeath = 32,
			DoubleTime = 64,
			Relax = 128,
			HalfTime = 256,
			Nightcore = 512, // Only set along with DoubleTime. i.e: NC only gives 576
			Flashlight = 1024,
			Autoplay = 2048,
			SpunOut = 4096,
			Relax2 = 8192,    // Autopilot
			Perfect = 16384, // Only set along with SuddenDeath. i.e: PF only gives 16416  
			Key4 = 32768,
			Key5 = 65536,
			Key6 = 131072,
			Key7 = 262144,
			Key8 = 524288,
			FadeIn = 1048576,
			Random = 2097152,
			Cinema = 4194304,
			Target = 8388608,
			Key9 = 16777216,
			KeyCoop = 33554432,
			Key1 = 67108864,
			Key3 = 134217728,
			Key2 = 268435456,
			ScoreV2 = 536870912,
			Mirror = 1073741824,
			KeyMod = Key1 | Key2 | Key3 | Key4 | Key5 | Key6 | Key7 | Key8 | Key9 | KeyCoop,
			FreeModAllowed = NoFail | Easy | Hidden | HardRock | SuddenDeath | Flashlight | FadeIn | Relax | Relax2 | SpunOut | KeyMod,
			ScoreIncreaseMods = Hidden | HardRock | DoubleTime | Flashlight | FadeIn
		}
	}
}
