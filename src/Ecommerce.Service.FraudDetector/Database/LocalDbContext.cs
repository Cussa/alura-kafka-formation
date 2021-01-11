using Ecommerce.Common;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Service.FraudDetector.Database
{
    public class LocalDbContext : DbContext
    {
        public LocalDbContext()
        {
            Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var databasePath = "frauds_database.db".WithCurrentDirectory();
            options.UseSqlite($"Data Source={databasePath}");
        }

        internal DbSet<DbOrder> Orders { get; set; }
    }
}
