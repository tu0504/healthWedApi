using Microsoft.EntityFrameworkCore;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
   public class ApplicationDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=CAMTU\\CAMTU;Database=Health;Uid=sa;Pwd=05042003;TrustServerCertificate=True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                new Role { RoleId = 1, RoleName = "Student" },
                new Role { RoleId = 2, RoleName = "Parent" },
                new Role { RoleId = 3, RoleName = "Psychologist" },
                new Role { RoleId = 4, RoleName = "Manager" }
            );

            modelBuilder.Entity<Account>().HasData(
                new Account
                {
                    AccountId = 1,
                    FullName = "John Doe",
                    Email = "johndoe@example.com",
                    Phone = "123-456-7890",
                    Address = "123 Main St, City",
                    PasswordHash = "hashedpassword123",
                    RoleId = 1,
                    IsDeleted = false
                },
                new Account
                {
                    AccountId = 2,
                    FullName = "Jane Smith",
                    Email = "janesmith@example.com",
                    Phone = "987-654-3210",
                    Address = "456 Elm St, City",
                    PasswordHash = "hashedpassword456",
                    RoleId = 2,
                    IsDeleted = false
                },
                new Account
                {
                    AccountId = 3,
                    FullName = "Alice Johnson",
                    Email = "alicejohnson@example.com",
                    Phone = "555-123-4567",
                    Address = "789 Oak St, City",
                    PasswordHash = "hashedpassword789",
                    RoleId = 3,
                    IsDeleted = false
                },
                new Account
                {
                    AccountId = 4,
                    FullName = "Bob Williams",
                    Email = "managerment@example.com",
                    Phone = "666-987-6543",
                    Address = "321 Pine St, City",
                    PasswordHash = "pass123",
                    RoleId = 4,
                    IsDeleted = false
                }
            );

        }
    }
}
