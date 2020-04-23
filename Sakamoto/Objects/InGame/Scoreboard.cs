using FFmpeg.AutoGen;
using Microsoft.EntityFrameworkCore.Internal;
using osu.Game.Overlays.BeatmapSet.Scores;
using Sakamoto.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sakamoto.Objects.InGame
{
	public class Scoreboard
	{
		public Beatmap beatmap;
		public List<Score> scores;
		public Scoreboard(Beatmap beatmap, List<Score> scores)
		{
			this.beatmap = beatmap;
			this.scores = scores;
		}
		public string AppendToString(int userid = -1, bool is_pp = false)
		{
			StringBuilder sb = new StringBuilder();
			if (beatmap == null)
			{
				return $"{(int)BeatmapSubmissionStatus.NotSubmitted}|false";
			} else
			{
				sb.Append(beatmap.AppendToString());
			}
			if (scores != null)
			{
				// Get Personal Score.
				if (userid != -1)
				{
					Score _ownscore = scores.Where(a => a.userid == userid).FirstOrDefault();
					if (_ownscore != null)
					{
						sb.Append(_ownscore.
							AppendToString(
							scores.FindIndex(a => a ==_ownscore).ToString()
							));
					}
				}
				// Get Top 50 Scores
				for (int i = 0; i < scores.Count; i++)
				{
					if (i > 50) break;
					Score s = scores[i];
					sb.Append(s.AppendToString(i.ToString()));
				}
			}
			
			
			return sb.ToString();
		}
	}
}
