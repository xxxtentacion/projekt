using Authentication.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Core.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<License> Licenses { get; set; }
        public DbSet<GameStats> GameStats { get; set; }
        public DbSet<LicenseStats> LicenseStats { get; set; }
    }
}
