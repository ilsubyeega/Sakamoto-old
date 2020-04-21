using osu.Game.Rulesets.Scoring;
using Sakamoto.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Sakamoto.Test.Tests
{
	public static class Calculate
	{
		public static int beatmapid = 1764213; // harumachi clover
		public static void Init()
		{
			Sakamoto.Common.path_beatmaps = Path.Combine(@"C:\Users\ilsubyeega\Documents", "Beatmaps");
			CalcDiff(); // 5.52*
			CalcPP(); // Accroding to StreamCompanion = 197
		}
		public static void CalcPP()
		{
			Dictionary<HitResult, int> stats = new Dictionary<HitResult, int>
				{
					{ HitResult.Perfect, 47 }, // geki
					{ HitResult.Great, 116 }, // 300
					{ HitResult.Good, 0 }, // 100
					{ HitResult.Ok, 0 }, // katu
					{ HitResult.Meh, 0 }, // 50
					{ HitResult.Miss, 0 } // miss
				};
			Console.WriteLine("Performance Calculation Result: {0}", Sakamoto.Util.Osu.PerformanceCalculator.Calculate(1537198, 0, (int)Mods.DoubleTime, 151, stats));
		}
		public static void CalcDiff()
		{
			Console.WriteLine("Diffculty Calculation Result: {0}", Sakamoto.Util.Osu.DifficultyCalculator.Calculate(beatmapid, 0, 0));
		}
	}
}
