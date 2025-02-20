using HEALTH_SUPPORT.Repositories.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Repositories
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<SubscriptionData> SubscriptionDatas { get; set; }
        public DbSet<SubscriptionProgress> SubscriptionProgresses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SurveyType> SurveyTypes { get; set; }
        public DbSet<SurveyQuestion> SurveyQuestions { get; set; }
        public DbSet<SurveyAnswer> SurveyAnswers { get; set; }
        public DbSet<SurveyResults> SurveyResults { get; set; }
        public DbSet<AccountSurvey> AccountSurveys { get; set; }
        public DbSet<Appointment> Appointments { get; set; }



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

            //modelBuilder.Entity<SurveyType>().HasKey(st => st.Id);

            //modelBuilder.Entity<Survey>().HasKey(s => s.Id);

            //Account-survey(m-m: AccountSurvey)
            modelBuilder.Entity<Account>().HasMany(s => s.AccountSurveys).WithOne(a => a.Account).HasForeignKey(s => s.AccountId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Survey>().HasMany(s => s.AccountSurveys).WithOne(a => a.Survey).HasForeignKey(s => s.SurveyId).OnDelete(DeleteBehavior.Restrict);

            //Survey-SurveyResults(1-m)
            modelBuilder.Entity<Survey>().HasMany(s => s.SurveyResults).WithOne(a => a.Survey).HasForeignKey(s => s.SurveyId).OnDelete(DeleteBehavior.Restrict);

            //SurveyResults-AccountSurvey(m-1)
            modelBuilder.Entity<AccountSurvey>().HasMany(s => s.SurveyResults).WithOne(a => a.AccountSurvey).HasForeignKey(s => s.AccountSurveyId).OnDelete(DeleteBehavior.Restrict);

            //Survey-SurveyQuestion(1-m)
            modelBuilder.Entity<Survey>().HasMany(s => s.SurveyQuestions).WithOne(a => a.Survey).HasForeignKey(s => s.SurveyId).OnDelete(DeleteBehavior.Restrict);

            //SurveyQuestion-SurveyAnswer(1-m)
            modelBuilder.Entity<SurveyQuestion>().HasMany(s => s.SurveyAnswers).WithOne(a => a.SurveyQuestion).HasForeignKey(s => s.QuestionId).OnDelete(DeleteBehavior.Restrict);

            //SurveyType-Survey(1-m)
            modelBuilder.Entity<SurveyType>().HasMany(s => s.Surveys).WithOne(a => a.SurveyType).HasForeignKey(s => s.SurveyTpyeId).OnDelete(DeleteBehavior.Restrict);

            //SurveyType-SurveyQuestion(1-m)
            modelBuilder.Entity<SurveyType>().HasMany(s => s.SurveyQuestions).WithOne(a => a.SurveyType).HasForeignKey(s => s.SurveyTypeId).OnDelete(DeleteBehavior.Restrict);

            //account - appointment
            modelBuilder.Entity<Account>().HasMany(t => t.Appointments).WithOne(a => a.Account).HasForeignKey(t => t.AccountId).OnDelete(DeleteBehavior.Restrict);

            //account - healthData(m-m)
            modelBuilder.Entity<Account>().HasMany(s => s.HealthDatas).WithOne(a => a.Account).HasForeignKey(s => s.AccountId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Psychologist>().HasMany(s => s.HealthDatas).WithOne(a => a.Psychologist).HasForeignKey(s => s.PsychologistId).OnDelete(DeleteBehavior.Restrict);



            modelBuilder.Entity<Account>().HasMany(t => t.Transactions).WithOne(a => a.Account).HasForeignKey(t => t.AccountId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transaction>().HasMany(o => o.Orders).WithOne(t => t.Transaction).HasForeignKey(o => o.TransactionId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Category>().HasMany(d => d.SubscriptionDatas).WithOne(c => c.Category).HasForeignKey(d => d.CategoryId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SubscriptionData>().HasMany(o => o.Orders).WithOne(d => d.Subscription).HasForeignKey(o => o.SubscriptionId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SubscriptionData>().HasMany(p => p.SubscriptionProgresses).WithOne(d => d.Subscription).HasForeignKey(p => p.SubscriptionId).OnDelete(DeleteBehavior.Restrict);
        }

    }
}
