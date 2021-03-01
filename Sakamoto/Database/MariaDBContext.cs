using Microsoft.EntityFrameworkCore;
using Sakamoto.Database.Models;
using Sakamoto.Database.Models.Beatmap;
using Sakamoto.Database.Models.Chat;
using Sakamoto.Database.Models.Group;
using Sakamoto.Database.Models.Legacy;
using Sakamoto.Database.Models.OAuth;

namespace Sakamoto.Database
{
	public class MariaDBContext : DbContext
	{
		public MariaDBContext(DbContextOptions<MariaDBContext> options)
			: base(options)
		{
		}

		public DbSet<DBUser> Users { get; set; }
		public DbSet<DBUserStat> UserStats { get; set; }

		// Beatmap
		public DbSet<DBBeatmap> Beatmaps { get; set; }
		public DbSet<DBBeatmapSet> BeatmapSets { get; set; }
		public DbSet<DBBeatmapsetFavourite> MapFavourites { get; set; }

		// Oauth
		public DbSet<DBAccessToken> AccessTokens { get; set; }
		public DbSet<DBAuthCode> AuthCodes { get; set; }
		public DbSet<DBClient> Clients { get; set; }
		public DbSet<DBPersonalAccessToken> PersonalAccessTokens { get; set; }
		public DbSet<DBRefreshToken> RefreshTokens { get; set; }

		// Chat
		public DbSet<DBChannel> Channels { get; set; }
		public DbSet<DBChannelPM> PMChannels { get; set; }
		public DbSet<DBMessage> Messages { get; set; }
		public DbSet<DBUserChannel> UserChannels { get; set; }

		// Groups
		public DbSet<DBGroup> Groups { get; set; }
		public DbSet<DBUserGroup> UserGroups { get; set; }

		// Legacy
		public DbSet<DBLegacyScore> LegacyScores { get; set; }


		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.Entity<DBUserGroup>().HasKey(table => new {
				table.GroupId, table.UserId
			});
			
			builder.Entity<DBBeatmapsetFavourite>().HasKey(table => new
			{
				table.BeatmapsetId,
				table.UserId
			});
			builder.Entity<DBBeatmapsetFavourite>().Property(p => p.FavouritedAt);
			
		}

	}
}
