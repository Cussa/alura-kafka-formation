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
            => options.UseSqlite("Data Source=users_database.db");

        internal DbSet<User> Users { get; set; }
    }
}
