using System.IO;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Service.Users
{
    public class UsersDbContext : DbContext
    {
        public UsersDbContext()
        {
            Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var databasePath = Path.Combine(Directory.GetCurrentDirectory(), "users_database.db");
            options.UseSqlite($"Data Source={databasePath}");
        }

        internal DbSet<User> Users { get; set; }
    }
}
