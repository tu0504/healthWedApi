using HEALTH_SUPPORT.Repositories.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Repositories
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<ProgramData> ProgramDatas { get; set; }
        public DbSet<ProgramProgress> ProgramProgresses { get; set; }
        public DbSet<ProgramRegistration> ProgramRegistrations { get; set; }
        public DbSet<SurveyType> SurveyTypes { get; set; }
        public DbSet<SurveyQuestion> SurveyQuestions { get; set; }
        public DbSet<SurveyAnswer> SurveyAnswers { get; set; }
        public DbSet<SurveyResults> SurveyResults { get; set; }
        public DbSet<AccountSurvey> AccountSurveys { get; set; }



        private readonly IConfiguration _configuration;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration)
        : base(options)
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().HasIndex(a => a.Email).IsUnique();
            modelBuilder.Entity<Role>().HasIndex(a => a.Name).IsUnique();
            modelBuilder.Entity<Account>().HasMany(p => p.ProgramProgresses).WithOne(a => a.Account).HasForeignKey(p => p.AccountId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ProgramData>().HasMany(p => p.ProgramProgresses).WithOne(a => a.ProgramData).HasForeignKey(p => p.ProgramDataId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Account>().HasMany(p => p.ProgramRegistrations).WithOne(a => a.Account).HasForeignKey(p => p.AccountId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ProgramData>().HasMany(p => p.ProgramRegistrations).WithOne(a => a.ProgramData).HasForeignKey(p => p.ProgramDataId).OnDelete(DeleteBehavior.Restrict);


        }

    }
}
