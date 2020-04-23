using osu.Shared;
using Sakamoto.Enums;
using System;

namespace Sakamoto.Objects.InGame
{
	public class Beatmap
	{
		// those two is romanised.
		public string title;
		public string artist;
		public string creator;
		public string diff;
		public GameMode mode;

		public string file_md5;
		public string filename;
		public BeatmapSubmissionStatus rankedstatus = BeatmapSubmissionStatus.NotSubmitted;

		public int beatmapid;
		public int beatmapsetid;

		public int totalscores = 0;
		public int offset = 0;
		public double rating = 10.0;

		public bool hasvideo;

		public Beatmap() { }
		public Beatmap(String input)
		{

		}
		public string AppendToString()
		{
			string a = $"{(int)rankedstatus}|false";
			if (rankedstatus != BeatmapSubmissionStatus.NotSubmitted &&
				rankedstatus != BeatmapSubmissionStatus.Unknown &&
				rankedstatus != BeatmapSubmissionStatus.Need_Update)
			{
				a += $"|{beatmapid}|{beatmapsetid}|{totalscores}\n{offset}\n{artist} - {title}\n{rating}\n";
			}
			return a;
		}
		public string AppendToString(BeatmapSubmissionStatus ranked_status)
		{
			string a = $"{(int)ranked_status}|false";
			if (ranked_status != BeatmapSubmissionStatus.NotSubmitted &&
				ranked_status != BeatmapSubmissionStatus.Unknown &&
				ranked_status != BeatmapSubmissionStatus.Need_Update)
			{
				a += $"|{beatmapid}|{beatmapsetid}|{totalscores}\n{offset}\n{artist} - {title} [{diff}]\n{rating}\n";
			}
			return a;
		}
		public string toDirect()
		{
			return $"{beatmapsetid}.osz|{artist}|{title}|{creator}|{(int)rankedstatus}|10.00|{Common.timestring}|{beatmapsetid}|{beatmapsetid}|{(hasvideo == true ? 1 : 0)}|1|1|7331";
		}
		public string toDirectDiff()
		{
			return $"{diff}@{(int)mode}";
		}
	}
}
