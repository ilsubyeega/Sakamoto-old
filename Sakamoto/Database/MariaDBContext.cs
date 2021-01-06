using Microsoft.EntityFrameworkCore;
using Sakamoto.Database.Models;
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
    }
}
