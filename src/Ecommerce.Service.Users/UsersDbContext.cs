﻿using Ecommerce.Common;
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
            var databasePath = "users_database.db".WithCurrentDirectory();
            options.UseSqlite($"Data Source={databasePath}");
        }

        internal DbSet<User> Users { get; set; }
    }
}
