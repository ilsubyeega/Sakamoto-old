using Microsoft.EntityFrameworkCore;
using Sakamoto.Database.Models;
using Sakamoto.Database.Models.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sakamoto.Database
{
    public class MariaDBContext : DbContext
    {
        public MariaDBContext(DbContextOptions<MariaDBContext> options)
            : base(options)
        {
        }

        public DbSet<DBUser> Users { get; set; }


        // Oauth
        public DbSet<DBAccessToken> AccessTokens { get; set; }
        public DbSet<DBAuthCode> AuthCodes { get; set; }
        public DbSet<DBClient> Clients { get; set; }
        public DbSet<DBPersonalAccessToken> PersonalAccessTokens { get; set; }
        public DbSet<DBRefreshToken> RefreshTokens { get; set; }
    }
}
