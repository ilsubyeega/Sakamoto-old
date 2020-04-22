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
		public static int beatmapid = 954692; // Kira Kira Days

		public static void Init()
		{
			Sakamoto.Common.path_beatmaps = Path.Combine(@"C:\Users\ilsubyeega\Documents", "Beatmaps");
			Console.WriteLine("FullObj: {0}", Sakamoto.Util.Osu.ScoreStatusUtil.GetHitObjectsCount(Sakamoto.Util.Osu.ScoreStatusUtil.GetBeatmap(beatmapid, 0, 0)));
			Console.WriteLine("Max Combo: {0}", Sakamoto.Util.Osu.ScoreStatusUtil.GetMaxCombo(Sakamoto.Util.Osu.ScoreStatusUtil.GetBeatmap(beatmapid, 0, 0)));
			CalcDiff(); // 6.04*
			CalcPP(); // Accroding to StreamCompanion = 277
		}
		public static void CalcPP()
		{
			Dictionary<HitResult, int> stats = new Dictionary<HitResult, int>
				{
					{ HitResult.Great, 401 }, // 300
					{ HitResult.Perfect, 75 }, // geki
					{ HitResult.Good, 6 }, // 100
					{ HitResult.Ok, 4 }, // katu
					{ HitResult.Meh, 0 }, // 50
					{ HitResult.Miss, 0 } // miss
				};
			Console.WriteLine("Performance Calculation Result: {0}", Sakamoto.Util.Osu.PerformanceCalculator.Calculate(beatmapid, 0, 0, 574, stats));
			
			stats = new Dictionary<HitResult, int>
				{
					{ HitResult.Great, 0 }, // 300
					{ HitResult.Perfect, 0 }, // geki
					{ HitResult.Good, 0 }, // 100
					{ HitResult.Ok, 0 }, // katu
					{ HitResult.Meh, 0 }, // 50
					{ HitResult.Miss, 407 } // miss
				};
			Console.WriteLine("Performance Calculation 0% Result: {0}", Sakamoto.Util.Osu.PerformanceCalculator.Calculate(beatmapid, 0, 0, 0, stats));
		}
		public static void CalcDiff()
		{
			Console.WriteLine("Diffculty Calculation Result: {0}", Sakamoto.Util.Osu.DifficultyCalculator.Calculate(beatmapid, 0, 0));
		}
	}
}
