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
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }
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
        public DbSet<SurveyQuestionAnswer> SurveyQuestionAnswer { get; set; }
        public DbSet<SurveyQuestionSurvey> SurveyQuestionSurvey { get; set; }
        public DbSet<SurveyAnswerRecord> SurveyAnswerRecord { get; set; }
        public DbSet<UserProgress> UserProgresses { get; set; }

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
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = Guid.Parse("2a5f5c96-cb79-40d4-a604-d484b7041e7f"), Name = "Manager"},
                new Role { Id = Guid.Parse("7d9d691a-58dc-48fd-9204-ffe02c4fd0fd"), Name = "Student" },
                new Role { Id = Guid.Parse("b6286c3e-1e4b-41ce-81e5-cc9a27ffe2e7"), Name = "Parent" },
                new Role { Id = Guid.Parse("5fff93bf-2324-425b-8f04-6a80af3bb0d3"), Name = "Psychologist" }
            );

            modelBuilder.Entity<Account>().HasData(
                new Account
                {
                    Id = Guid.Parse("5b0884a0-0067-49f5-b3be-a29ef58aa70c"),
                    UserName = "Manager1",
                    Fullname = "Manager1 nè",
                    Email = "admin@example.com",
                    Phone = "0123456789",
                    Address = "123 Admin Street",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                    CreateAt = DateTimeOffset.UtcNow,
                    LoginDate = DateTimeOffset.UtcNow,
                    RoleId = Guid.Parse("2a5f5c96-cb79-40d4-a604-d484b7041e7f")
                },
                new Account
                {
                    Id = Guid.Parse("dad2a80f-70e4-49f6-b3c5-3c1eedf525e4"),
                    UserName = "Manager2",
                    Fullname = "Manager2 nè",
                    Email = "admin2@example.com",
                    Phone = "0123456789",
                    Address = "123 Admin Street",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@456"),
                    CreateAt = DateTimeOffset.UtcNow,
                    LoginDate = DateTimeOffset.UtcNow,
                    RoleId = Guid.Parse("2a5f5c96-cb79-40d4-a604-d484b7041e7f")
                },
                new Account
                {
                    Id = Guid.Parse("05e5e6d4-f866-447c-b610-46fc721e09cd"),
                    UserName = "Hanh",
                    Fullname = "LÊ THẾ HANH",
                    Email = "thehanh@gmail.com",
                    Phone = "1234567890",
                    Address = "123 Main St",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"),
                    IsEmailVerified = true,
                    CreateAt = DateTimeOffset.UtcNow,
                    LoginDate = DateTimeOffset.UtcNow,
                    RoleId = Guid.Parse("5fff93bf-2324-425b-8f04-6a80af3bb0d3")
                },
                new Account
                {
                    Id = Guid.Parse("880f0367-847a-4406-8146-7bff47cd36ec"),
                    UserName = "TƯ",
                    Fullname = "PHẠM VĂN TƯ",
                    Email = "janesmith@example.com",
                    Phone = "0987654321",
                    Address = "456 Elm St",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"),
                    IsEmailVerified = true,
                    CreateAt = DateTimeOffset.UtcNow,
                    LoginDate = DateTimeOffset.UtcNow,
                    RoleId = Guid.Parse("5fff93bf-2324-425b-8f04-6a80af3bb0d3")
                },
                new Account
                {
                    Id = Guid.Parse("aa36fb5b-a181-4a64-88b2-a9c1c698801c"),
                    UserName = "Thắm",
                    Fullname = "Nguyễn Thị Thắm",
                    Email = "psy3@gmail.com",
                    Phone = "0987654321",
                    Address = "456 Elm St",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"),
                    IsEmailVerified = true,
                    CreateAt = DateTimeOffset.UtcNow,
                    LoginDate = DateTimeOffset.UtcNow,
                    RoleId = Guid.Parse("5fff93bf-2324-425b-8f04-6a80af3bb0d3")
                },
                new Account
                {
                    Id = Guid.Parse("6f95ed65-e4b3-491d-849b-7491101e2264"),
                    UserName = "Phượng",
                    Fullname = "Nguyễn Minh Phượng",
                    Email = "psy4@gmail.com",
                    Phone = "0987654321",
                    Address = "456 Elm St",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"),
                    IsEmailVerified = true,
                    CreateAt = DateTimeOffset.UtcNow,
                    LoginDate = DateTimeOffset.UtcNow,
                    RoleId = Guid.Parse("5fff93bf-2324-425b-8f04-6a80af3bb0d3")
                },
                new Account
                {
                    Id = Guid.Parse("078afb32-4351-44f7-bc0c-742e0e3b5c9f"),
                    UserName = "Oanh",
                    Fullname = "Vũ Thị Oanh",
                    Email = "psy5@gmail.com",
                    Phone = "0987654321",
                    Address = "456 Elm St",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"),
                    IsEmailVerified = true,
                    CreateAt = DateTimeOffset.UtcNow,
                    LoginDate = DateTimeOffset.UtcNow,
                    RoleId = Guid.Parse("5fff93bf-2324-425b-8f04-6a80af3bb0d3")
                }
            );
            modelBuilder.Entity<Account>().HasIndex(a => a.UserName).IsUnique();

            modelBuilder.Entity<SurveyType>().HasData(
                new SurveyType { Id = Guid.Parse("b23f0870-f5d9-463f-8ffc-98a133da47e8"), SurveyName = "Đánh giá lo âu (GAD-7)" },
                new SurveyType { Id = Guid.Parse("b24fc1c5-ff98-4767-adb6-56b57f91b22e"), SurveyName = "Đánh giá trầm cảm (PHQ-9)" }
            );

            modelBuilder.Entity<Survey>().HasData(
                new Survey
                {
                    Id = Guid.Parse("418be23d-8db9-4b74-a86a-9402f246ea62"),
                    MaxScore = 21,
                    CreateAt = DateTimeOffset.UtcNow,
                    SurveyTypeId = Guid.Parse("b23f0870-f5d9-463f-8ffc-98a133da47e8")
                },
                new Survey
                {
                    Id = Guid.Parse("86deeb52-2ef9-47d9-8496-edac723ffbf7"),
                    MaxScore = 27,
                    CreateAt = DateTimeOffset.UtcNow,
                    SurveyTypeId = Guid.Parse("b24fc1c5-ff98-4767-adb6-56b57f91b22e")
                }
            );

            modelBuilder.Entity<SurveyQuestion>().HasData(
                new SurveyQuestion
                {
                    Id = Guid.Parse("07cb8575-7b7d-4784-a6b2-9cc1543f0267"),
                    ContentQ = "Cảm thấy lo lắng, căng thẳng, hoặc bồn chồn.",
                    CreateAt = DateTimeOffset.UtcNow,
                    SurveyTypeId = Guid.Parse("b23f0870-f5d9-463f-8ffc-98a133da47e8")
                },
                new SurveyQuestion
                {
                    Id = Guid.Parse("798b80ac-f766-4baf-aa01-dbebca21f98b"),
                    ContentQ = "Không thể dừng hoặc kiểm soát sự lo lắng.",
                    CreateAt = DateTimeOffset.UtcNow,
                    SurveyTypeId = Guid.Parse("b23f0870-f5d9-463f-8ffc-98a133da47e8")
                },
                new SurveyQuestion
                {
                    Id = Guid.Parse("0998c44c-7a8d-41fd-8b3e-05d775ce4b27"),
                    ContentQ = "Lo lắng quá nhiều về những điều khác nhau.",
                    CreateAt = DateTimeOffset.UtcNow,
                    SurveyTypeId = Guid.Parse("b23f0870-f5d9-463f-8ffc-98a133da47e8")
                },
                new SurveyQuestion
                {
                    Id = Guid.Parse("7b1cc80a-8c8f-4695-99eb-271c46b9c766"),
                    ContentQ = "Khó thư giãn.",
                    CreateAt = DateTimeOffset.UtcNow,
                    SurveyTypeId = Guid.Parse("b23f0870-f5d9-463f-8ffc-98a133da47e8")
                },
                new SurveyQuestion
                {
                    Id = Guid.Parse("a1ce2d59-88d4-463d-8af2-f472b5771746"),
                    ContentQ = "Bồn chồn đến mức khó ngồi yên.",
                    CreateAt = DateTimeOffset.UtcNow,
                    SurveyTypeId = Guid.Parse("b23f0870-f5d9-463f-8ffc-98a133da47e8")
                },
                new SurveyQuestion
                {
                    Id = Guid.Parse("a9ffedd5-3f8e-4d98-b230-ae2491045f0d"),
                    ContentQ = "Dễ cáu gắt hoặc bực mình.",
                    CreateAt = DateTimeOffset.UtcNow,
                    SurveyTypeId = Guid.Parse("b23f0870-f5d9-463f-8ffc-98a133da47e8")
                },
                new SurveyQuestion
                {
                    Id = Guid.Parse("4f4736e4-ecbe-4bdc-a9cf-cac7db918aae"),
                    ContentQ = "Cảm giác như có điều gì đó khủng khiếp sẽ xảy ra.",
                    CreateAt = DateTimeOffset.UtcNow,
                    SurveyTypeId = Guid.Parse("b23f0870-f5d9-463f-8ffc-98a133da47e8")
                },
                new SurveyQuestion
                {
                    Id = Guid.Parse("59d5f97c-11b1-40d3-a69c-6c3700ff2c7d"),
                    ContentQ = "Ít hứng thú hoặc niềm vui khi làm mọi việc.",
                    CreateAt = DateTimeOffset.UtcNow,
                    SurveyTypeId = Guid.Parse("b24fc1c5-ff98-4767-adb6-56b57f91b22e")
                },
                new SurveyQuestion
                {
                    Id = Guid.Parse("37a557b5-c10a-4062-b7e5-0b2f4e02636e"),
                    ContentQ = "Cảm thấy buồn, chán nản, hoặc tuyệt vọng.",
                    CreateAt = DateTimeOffset.UtcNow,
                    SurveyTypeId = Guid.Parse("b24fc1c5-ff98-4767-adb6-56b57f91b22e")
                },
                new SurveyQuestion
                {
                    Id = Guid.Parse("416fcdf0-e6e6-4e76-9e3a-cb80b9a57a49"),
                    ContentQ = "Khó ngủ hoặc ngủ quá nhiều.",
                    CreateAt = DateTimeOffset.UtcNow,
                    SurveyTypeId = Guid.Parse("b24fc1c5-ff98-4767-adb6-56b57f91b22e")
                },
                new SurveyQuestion
                {
                    Id = Guid.Parse("a36264c0-9ad8-4205-811b-a867d2c7966d"),
                    ContentQ = "Mệt mỏi hoặc thiếu năng lượng.",
                    CreateAt = DateTimeOffset.UtcNow,
                    SurveyTypeId = Guid.Parse("b24fc1c5-ff98-4767-adb6-56b57f91b22e")
                },
                new SurveyQuestion
                {
                    Id = Guid.Parse("fdce1e6a-d8dc-4ddc-bd63-9f9acf666467"),
                    ContentQ = "Chán ăn hoặc ăn quá nhiều.",
                    CreateAt = DateTimeOffset.UtcNow,
                    SurveyTypeId = Guid.Parse("b24fc1c5-ff98-4767-adb6-56b57f91b22e")
                },
                new SurveyQuestion
                {
                    Id = Guid.Parse("0ff7226b-02f5-45dd-b3a6-f3bb2c804c5d"),
                    ContentQ = "Cảm giác tệ về bản thân, hoặc cảm thấy mình là người thất bại.",
                    CreateAt = DateTimeOffset.UtcNow,
                    SurveyTypeId = Guid.Parse("b24fc1c5-ff98-4767-adb6-56b57f91b22e")
                },
                new SurveyQuestion
                {
                    Id = Guid.Parse("8f0a5958-04ad-403a-bbad-ea476f5c2cc5"),
                    ContentQ = "Khó tập trung vào công việc hoặc khi đọc báo/xem TV.",
                    CreateAt = DateTimeOffset.UtcNow,
                    SurveyTypeId = Guid.Parse("b24fc1c5-ff98-4767-adb6-56b57f91b22e")
                },
                new SurveyQuestion
                {
                    Id = Guid.Parse("3f0dce08-ebbd-4872-b9dc-d3de0b66279f"),
                    ContentQ = "Cử động hoặc nói chậm hơn bình thường, hoặc ngược lại quá bồn chồn.",
                    CreateAt = DateTimeOffset.UtcNow,
                    SurveyTypeId = Guid.Parse("b24fc1c5-ff98-4767-adb6-56b57f91b22e")
                },
                new SurveyQuestion
                {
                    Id = Guid.Parse("41ecf897-7bfa-4892-abb8-eee9aacc7eb5"),
                    ContentQ = "Suy nghĩ về việc tự làm hại bản thân hoặc muốn chết.",
                    CreateAt = DateTimeOffset.UtcNow,
                    SurveyTypeId = Guid.Parse("b24fc1c5-ff98-4767-adb6-56b57f91b22e")
                }
            );

            modelBuilder.Entity<SurveyAnswer>().HasData(
                new SurveyAnswer
                {
                    Id = Guid.Parse("28dee921-db63-45c3-87a3-bdf0a263370d"),
                    Content = "Không hề",
                    Point = 0,
                    CreateAt = DateTimeOffset.UtcNow
                },
                new SurveyAnswer
                {
                    Id = Guid.Parse("66d927b9-e3c2-47f3-8aa9-82260d1579cd"),
                    Content = "Vài ngày",
                    Point = 1,
                    CreateAt = DateTimeOffset.UtcNow
                },
                new SurveyAnswer
                {
                    Id = Guid.Parse("91c14006-832a-41dd-8d03-e7950cf54347"),
                    Content = "Hơn một nửa số ngày",
                    Point = 2,
                    CreateAt = DateTimeOffset.UtcNow
                },
                new SurveyAnswer
                {
                    Id = Guid.Parse("f7095907-611f-4ed9-b622-5df984f48175"),
                    Content = "Gần như mỗi ngày",
                    Point = 3,
                    CreateAt = DateTimeOffset.UtcNow
                }
            );

            modelBuilder.Entity<SurveyResults>().HasData(
                new SurveyResults
                {
                    Id = Guid.Parse("e43de268-c18c-43f7-a219-b4f072d3043e"),
                    MinScore = 0,
                    MaxScore = 4,
                    ResultDescription = "Không hoặc rất nhẹ",
                    CreateAt = DateTimeOffset.UtcNow,
                    SurveyId = Guid.Parse("418be23d-8db9-4b74-a86a-9402f246ea62")
                },
                new SurveyResults
                {
                    Id = Guid.Parse("b0e3f7a4-47b3-470c-9612-f7030c9452ac"),
                    MinScore = 5,
                    MaxScore = 9,
                    ResultDescription = "Nhẹ",
                    CreateAt = DateTimeOffset.UtcNow,
                    SurveyId = Guid.Parse("418be23d-8db9-4b74-a86a-9402f246ea62")
                },
                new SurveyResults
                {
                    Id = Guid.Parse("c7986ab8-672b-4bb2-ba0f-791224a29100"),
                    MinScore = 10,
                    MaxScore = 14,
                    ResultDescription = "Vừa",
                    CreateAt = DateTimeOffset.UtcNow,
                    SurveyId = Guid.Parse("418be23d-8db9-4b74-a86a-9402f246ea62")
                },
                new SurveyResults
                {
                    Id = Guid.Parse("2d903f3f-dbd1-4266-b486-5a6ea11dd90d"),
                    MinScore = 15,
                    MaxScore = 21,
                    ResultDescription = "Nặng",
                    CreateAt = DateTimeOffset.UtcNow,
                    SurveyId = Guid.Parse("418be23d-8db9-4b74-a86a-9402f246ea62")
                },


                new SurveyResults
                {
                    Id = Guid.Parse("d9156493-2f6a-47fc-9fb9-4af265e3b182"),
                    MinScore = 0,
                    MaxScore = 4,
                    ResultDescription = "Không hoặc rất nhẹ",
                    CreateAt = DateTimeOffset.UtcNow,
                    SurveyId = Guid.Parse("86deeb52-2ef9-47d9-8496-edac723ffbf7")
                },
                new SurveyResults
                {
                    Id = Guid.Parse("4de4b9bf-0750-4960-b8b8-37e72d254d1e"),
                    MinScore = 5,
                    MaxScore = 9,
                    ResultDescription = "Nhẹ",
                    CreateAt = DateTimeOffset.UtcNow,
                    SurveyId = Guid.Parse("86deeb52-2ef9-47d9-8496-edac723ffbf7")
                },
                new SurveyResults
                {
                    Id = Guid.Parse("8e8b719f-0e0b-4dff-9188-c95e9d11ad0c"),
                    MinScore = 10,
                    MaxScore = 14,
                    ResultDescription = "Vừa",
                    CreateAt = DateTimeOffset.UtcNow,
                    SurveyId = Guid.Parse("86deeb52-2ef9-47d9-8496-edac723ffbf7")
                },
                new SurveyResults
                {
                    Id = Guid.Parse("5faf9d3c-2713-411f-992e-a61aeb6207f9"),
                    MinScore = 15,
                    MaxScore = 19,
                    ResultDescription = "Nặng",
                    CreateAt = DateTimeOffset.UtcNow,
                    SurveyId = Guid.Parse("86deeb52-2ef9-47d9-8496-edac723ffbf7")
                },
                new SurveyResults
                {
                    Id = Guid.Parse("f2b2b3b4-1b3b-4b3b-8b3b-9b3b3b3b3b3b"),
                    MinScore = 20,
                    MaxScore = 27,
                    ResultDescription = "Rất nặng",
                    CreateAt = DateTimeOffset.UtcNow,
                    SurveyId = Guid.Parse("86deeb52-2ef9-47d9-8496-edac723ffbf7")
                }

            );

            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    Id = Guid.Parse("cea27467-163a-4248-8bdf-aec1429a4e6c"),
                    CategoryName = "Cảm xúc",
                    Description = "Giúp học sinh nhận diện, kiểm soát cảm xúc và tự điều chỉnh.."
                },
                new Category
                {
                    Id = Guid.Parse("3057a024-6931-440e-9ad0-c3e8fdbcaa92"),
                    CategoryName = "Tư duy",
                    Description = "Rèn luyện tư duy lạc quan, tạo động lực, tự tin và giao tiếp hiệu quả."
                }
            );

            modelBuilder.Entity<Psychologist>().HasData(
                new Psychologist
                {
                    Id = Guid.Parse("316d922e-5cdd-4df0-bc2e-744b2aa6b42e"),
                    Name = "Lê Thế Hanh",
                    Email = "thehanh@gmail.com",
                    PhoneNumber = "1234567890",
                    Specialization = "Thạc sĩ Tâm lý học Lâm sàng",
                    Description = "Chuyên gia tham vấn trị liệu tâm lý cho thanh thiếu niên. Chuyên gia tâm lý trong lĩnh vực hàn gắn các mối quan hệ, trị liệu các rối loạn tâm thần. Chuyên gia đào tạo tập huấn chương trình dự phòng sang chấn tâm lý tại trường học, tổ chức. Chuyên viên điều trị chứng nghiện, đặc biệt là nghiện Game/Internet",
                    Achievements = "Rối loạn chức năng gia đình, chữa lành mối quan hệ cặp đôi, mối quan hệ cha mẹ con cái, can thiệp hệ thống gia đình. Rối loạn hành vi lứa tuổi trẻ em và vị thành niên (hành vi chống đối, hành vi lạm dụng chất và lạm dụng công nghệ).",
                    Expertise = "Rối loạn trầm cảm, rối loạn lo âu, rối loạn căng thăng.",
                    ImgUrl = "/uploads/1.jpg",
                    CreateAt = DateTimeOffset.UtcNow,
                    AccountId = Guid.Parse("05e5e6d4-f866-447c-b610-46fc721e09cd")
                },
                new Psychologist
                {
                    Id = Guid.Parse("257b73a3-d691-40d3-b65d-a56a0ad7fb91"),
                    Name = "Phạm Văn Tư",
                    Email = "janesmith@example.com",
                    PhoneNumber = "0987654321",
                    Specialization = "Tiến sĩ  – Chuyên gia Tâm lý học.",
                    Description = "Hơn 20 năm kinh nghiệm trong nghề, gồm đánh giá và trị liệu tâm lý cho thanh thiếu niên và người trưởng thành. Diễn giả, Chuyên viên huấn luyện cao cấp trong lĩnh vực chăm sóc sức khỏe tinh thần cho tổ chức, doanh nghiệp, trường học. Hiện tại Tiến sĩ đang công tác tại Trường Đại học Sư phạm Hà Nội.",
                    Achievements = "Đánh giá tâm lý các vấn đề liên quan sức khỏe tâm thần (stress, lo âu, trầm cảm, nguy cơ tự tử…); tham vấn vị thành niên và gia đình; hỗ trợ giáo viên, phụ huynh về phương pháp dạy trẻ; giảm thiểu stress.",
                    Expertise = "Stress, lo âu, trầm cảm.",
                    ImgUrl = "/uploads/2.jpg",
                    CreateAt = DateTimeOffset.UtcNow,
                    AccountId = Guid.Parse("880f0367-847a-4406-8146-7bff47cd36ec")
                },
                new Psychologist
                {
                    Id = Guid.Parse("c8f8f39b-87a6-4292-b9bf-bbc78ee57fc7"),
                    Name = "Nguyễn Thị Thắm",
                    Email = "psy3@gmail.com",
                    PhoneNumber = "0987654321",
                    Specialization = "Tiến sĩ giáo dục.",
                    Description = "Đã có gần 20 năm kinh nghiệm nghiên cứu và thực hành về lĩnh vực tâm lý và giáo dục, thực hiện đánh giá, tham vấn, trị liệu tâm lý cho trẻ em, vị thành niên, thanh thiếu niên về các vấn đề liên quan đến rối loạn phát triển, lo âu, trầm cảm, rối loạn cảm xúc hành vi, hành vi gây hấn, chống đối, rối loạn ám ảnh cưỡng chế, tư vấn định hướng nghề, ám sợ trường học, lạm dụng game internet, …",
                    Achievements = "Đánh giá, tư vấn và can thiệp, trị liệu tâm bệnh lý trẻ em, thanh thiếu niên, người trưởng thành về các lĩnh vực: \r\n– Tự kỷ \r\n– Rối loạn lo âu – Trầm cảm\r\n– Chậm phát triển trí tuệ\r\n – Tăng động giảm chú ý \r\n– Các rối loạn dạng cơ thể\r\n – Rối loạn hành vi trẻ em vị thành niên \r\n– Rối loạn sự thích ứng\r\n",
                    Expertise = "Tự kỷ, rối loạn lo âu,  trầm cảm.",
                    ImgUrl = "/uploads/3.jpg",
                    CreateAt = DateTimeOffset.UtcNow,
                    AccountId = Guid.Parse("aa36fb5b-a181-4a64-88b2-a9c1c698801c")
                },
                new Psychologist
                {
                    Id = Guid.Parse("35153fa7-8510-4813-8ba5-447c1398f455"),
                    Name = "Nguyễn Minh Phượng",
                    Email = "psy4@gmail.com",
                    PhoneNumber = "0987654321",
                    Specialization = "Tiến sĩ giáo dục – giáo dục đặc biệt",
                    Description = "Hiện đang là giảng viên của Trường Đại học Sư phạm Hà Nội. Hơn 15 năm kinh nghiệm công tác trong lĩnh vực giáo dục và giáo dục đặc biệt, bao gồm: đánh giá, can thiệp, trị liệu và tư vấn giáo dục cho trẻ em và thanh thiếu niên. Giảng dạy, tập huấn, bồi dưỡng chuyên môn cho giáo viên ở các địa phương về đánh giá, xây dựng kế hoạch giáo dục cá nhân, các phương pháp can thiệp, giáo dục trẻ có nhu cầu đặc biệt.",
                    Achievements = "Đánh giá xác định mức độ phát triển hiện tại của trẻ so với tuổi thực, từ đó xác định điểm mạnh và khó khăn, hạn chế của con để có phương pháp chăm sóc, giáo dục phù hợp, giúp con phát triển tối ưu.",
                    Expertise = "Tự kỷ, tăng động giảm chú ý.",
                    ImgUrl = "/uploads/4.jpg",
                    CreateAt = DateTimeOffset.UtcNow,
                    AccountId = Guid.Parse("6f95ed65-e4b3-491d-849b-7491101e2264")
                },
                new Psychologist
                {
                    Id = Guid.Parse("536fa24e-0ab4-4cf3-a563-424df74d638f"),
                    Name = "Vũ Thị Oanh",
                    Email = "psy5@gmail.com",
                    PhoneNumber = "0987654321",
                    Specialization = "Thạc sĩ chuyên ngành: Tâm lý học lâm sàng.",
                    Description = "Trên 15 năm kinh nghiệm trong lĩnh vực tham vấn và trị liệu tâm lý cho trẻ em, học sinh và người lớn.",
                    Achievements = "Thiết kế, thực hiện các hoạt động đào tạo, tư vấn, tham vấn, trị liệu trực tiếp đối với một số rối loạn về tâm lý như: Rối loạn hành vi, cảm xúc, lo âu, trầm cảm, OCD, rối loạn chống đối và tăng động giảm tập trung (ADHD), Stress, PTSD, rối loạn nhân cách,…",
                    Expertise = "Rối loạn hành vi, cảm xúc, lo âu, trầm cảm.",
                    ImgUrl = "/uploads/5.jpg",
                    CreateAt = DateTimeOffset.UtcNow,
                    AccountId = Guid.Parse("078afb32-4351-44f7-bc0c-742e0e3b5c9f")
                }
                );

            modelBuilder.Entity<SubscriptionData>().HasData(
                new SubscriptionData
                {
                    Id = Guid.Parse("eb47bd9c-8594-47ea-997f-eea3f34b4fe2"),
                    SubscriptionName = "Kiểm soát cảm xúc",
                    Description = "Chương trình này được thiết kế để giúp học sinh phát triển khả năng nhận diện và kiểm soát cảm xúc của mình, đặc biệt là những cảm xúc tiêu cực như căng thẳng, lo âu và tức giận. Học viên sẽ được hướng dẫn về các phương pháp quản lý cảm xúc, bao gồm kỹ thuật hít thở sâu, thiền định, tái cấu trúc suy nghĩ tiêu cực và rèn luyện khả năng kiểm soát hành vi trong các tình huống căng thẳng. Khóa học này đặc biệt phù hợp với học sinh thường xuyên gặp khó khăn trong việc kiểm soát cảm xúc hoặc dễ bị ảnh hưởng bởi áp lực học tập. Ngoài ra, các bài kiểm tra nhận diện cảm xúc, bảng theo dõi mức độ căng thẳng và báo cáo tiến trình hàng tuần sẽ giúp học viên theo dõi sự phát triển của bản thân một cách rõ ràng.",
                    Price = 1000000,
                    Duration = 30,
                    Purpose = "",
                    Criteria = "",
                    FocusGroup = "",
                    AssessmentTool = "",
                    CreateAt = DateTimeOffset.UtcNow,
                    CategoryId = Guid.Parse("cea27467-163a-4248-8bdf-aec1429a4e6c"),
                    PsychologistId = Guid.Parse("316d922e-5cdd-4df0-bc2e-744b2aa6b42e")
                },
                new SubscriptionData
                {
                    Id = Guid.Parse("8f436d13-ad84-4761-adb7-2049e907cd2b"),
                    SubscriptionName = "Xây dựng tư duy tích cực",
                    Description = "Khóa học này tập trung vào việc hướng dẫn học sinh rèn luyện tư duy tích cực, giúp họ duy trì thái độ sống lạc quan và xây dựng động lực để đạt được thành công. Nội dung chương trình bao gồm phương pháp thay đổi suy nghĩ tiêu cực, thực hành lòng biết ơn, đặt mục tiêu hiệu quả và phát triển sự kiên trì. Học sinh thường xuyên cảm thấy mất động lực, hay nghi ngờ bản thân hoặc có xu hướng suy nghĩ tiêu cực sẽ được hưởng lợi nhiều nhất từ khóa học này. Các bài tập đo lường mức độ tư duy tích cực, phản hồi từ giảng viên và kế hoạch phát triển cá nhân sẽ giúp học viên theo dõi sự thay đổi của mình qua từng tuần.",
                    Price = 1500000,
                    Duration = 30,
                    Purpose = "",
                    Criteria = "",
                    FocusGroup = "",
                    AssessmentTool = "",
                    CreateAt = DateTimeOffset.UtcNow,
                    CategoryId = Guid.Parse("3057a024-6931-440e-9ad0-c3e8fdbcaa92"),
                    PsychologistId = Guid.Parse("316d922e-5cdd-4df0-bc2e-744b2aa6b42e")
                },
                new SubscriptionData
                {
                    Id = Guid.Parse("4a580bd8-d04c-4980-87f9-456c92ca6471"),
                    SubscriptionName = "Kỹ năng giao tiếp",
                    Description = "Chương trình này giúp học sinh cải thiện khả năng giao tiếp hiệu quả trong nhiều tình huống khác nhau, từ việc giao tiếp hàng ngày đến các tình huống quan trọng như làm việc nhóm, thuyết trình và phỏng vấn xin việc. Nội dung bao gồm cách diễn đạt ý tưởng rõ ràng, kỹ thuật lắng nghe chủ động, sử dụng ngôn ngữ cơ thể phù hợp và xử lý tình huống giao tiếp khó khăn. Khóa học đặc biệt hữu ích cho những học sinh gặp khó khăn khi nói chuyện trước đám đông hoặc thiếu tự tin trong giao tiếp xã hội. Ngoài ra, học viên sẽ tham gia vào các bài kiểm tra kỹ năng giao tiếp, thực hành đối thoại và nhận phản hồi từ chuyên gia để theo dõi sự tiến bộ của mình.",
                    Price = 1700000,
                    Duration = 30,
                    Purpose = "",
                    Criteria = "",
                    FocusGroup = "",
                    AssessmentTool = "",
                    CreateAt = DateTimeOffset.UtcNow,
                    CategoryId = Guid.Parse("3057a024-6931-440e-9ad0-c3e8fdbcaa92"),
                    PsychologistId = Guid.Parse("257b73a3-d691-40d3-b65d-a56a0ad7fb91")
                },
                new SubscriptionData
                {
                    Id = Guid.Parse("a1c2f3b4-d5e6-47fa-8bcd-123456789abc"),
                    SubscriptionName = "Quản lý thời gian hiệu quả",
                    Description = "Khóa học này giúp học sinh rèn luyện kỹ năng quản lý thời gian, lập kế hoạch học tập và làm việc một cách khoa học. Học viên sẽ được hướng dẫn sử dụng các công cụ lập kế hoạch, xác định ưu tiên và tránh trì hoãn trong công việc. Nội dung khóa học bao gồm các phương pháp như kỹ thuật Pomodoro, ma trận Eisenhower và lập lịch hàng tuần. Đây là chương trình lý tưởng cho những học sinh gặp khó khăn trong việc sắp xếp công việc và luôn cảm thấy thiếu thời gian. Các bài tập thực hành, kế hoạch cá nhân hóa và đánh giá từ giảng viên sẽ giúp học viên cải thiện hiệu suất học tập một cách rõ ràng.",
                    Price = 1200000,
                    Duration = 30,
                    Purpose = "",
                    Criteria = "",
                    FocusGroup = "",
                    AssessmentTool = "",
                    CreateAt = DateTimeOffset.UtcNow,
                    CategoryId = Guid.Parse("3057a024-6931-440e-9ad0-c3e8fdbcaa92"),
                    PsychologistId = Guid.Parse("257b73a3-d691-40d3-b65d-a56a0ad7fb91")
                },
                new SubscriptionData
                {
                    Id = Guid.Parse("f4e3d2c1-b6a7-48fb-9cde-abcdef123456"),
                    SubscriptionName = "Giảm căng thẳng và lo âu",
                    Description = "Khóa học này giúp học sinh học cách kiểm soát căng thẳng và lo âu thông qua các phương pháp thư giãn khoa học. Nội dung bao gồm thực hành thiền định, yoga nhẹ, phương pháp hít thở sâu và kỹ thuật giảm áp lực tinh thần. Đây là chương trình phù hợp với học sinh cảm thấy căng thẳng trước kỳ thi hoặc dễ bị áp lực từ môi trường học tập. Ngoài các bài hướng dẫn, khóa học còn cung cấp các bài kiểm tra mức độ căng thẳng và công cụ theo dõi tiến trình để giúp học viên cải thiện tinh thần hiệu quả.",
                    Price = 1400000,
                    Duration = 30,
                    Purpose = "",
                    Criteria = "",
                    FocusGroup = "",
                    AssessmentTool = "",
                    CreateAt = DateTimeOffset.UtcNow,
                    CategoryId = Guid.Parse("cea27467-163a-4248-8bdf-aec1429a4e6c"),
                    PsychologistId = Guid.Parse("316d922e-5cdd-4df0-bc2e-744b2aa6b42e")
                }
            );


            modelBuilder.Entity<SubscriptionProgress>().HasData(
                // Progress for "Kiểm soát cảm xúc"
                new SubscriptionProgress
                {
                    Id = Guid.Parse("1354b694-834d-4d69-ba21-c60b94dc6daf"),
                    Section = 1,
                    Description = "Nhận diện và phân loại cảm xúc",
                    Date = 1,
                    SubscriptionId = Guid.Parse("eb47bd9c-8594-47ea-997f-eea3f34b4fe2"),
                    IsCompleted = false,
                    CreateAt = DateTimeOffset.UtcNow
                },
                new SubscriptionProgress
                {
                    Id = Guid.Parse("344ad3c0-ab74-4ed7-9c14-996caeba2af6"),
                    Section = 2,
                    Description = "Hiểu rõ nguyên nhân gây ra cảm xúc",
                    Date = 3,
                    SubscriptionId = Guid.Parse("eb47bd9c-8594-47ea-997f-eea3f34b4fe2"),
                    IsCompleted = false,
                    CreateAt = DateTimeOffset.UtcNow
                },
                new SubscriptionProgress
                {
                    Id = Guid.Parse("88ecef38-121a-4faa-ba7a-638e3f73a1ed"),
                    Section = 3,
                    Description = "Chiến lược kiểm soát cảm xúc tiêu cực",
                    Date = 6,
                    SubscriptionId = Guid.Parse("eb47bd9c-8594-47ea-997f-eea3f34b4fe2"),
                    IsCompleted = false,
                    CreateAt = DateTimeOffset.UtcNow
                },
                new SubscriptionProgress
                {
                    Id = Guid.Parse("73eb18fb-796a-4474-b35f-a85315636bfb"),
                    Section = 4,
                    Description = "Thực hành kỹ thuật thư giãn và thiền",
                    Date = 10,
                    SubscriptionId = Guid.Parse("eb47bd9c-8594-47ea-997f-eea3f34b4fe2"),
                    IsCompleted = false,
                    CreateAt = DateTimeOffset.UtcNow
                },
                new SubscriptionProgress
                {
                    Id = Guid.Parse("2c04483d-a1b7-4771-ab49-330e46236529"),
                    Section = 5,
                    Description = "Áp dụng vào thực tế: Tự kiểm soát cảm xúc",
                    Date = 10,
                    SubscriptionId = Guid.Parse("eb47bd9c-8594-47ea-997f-eea3f34b4fe2"),
                    IsCompleted = false,
                    CreateAt = DateTimeOffset.UtcNow
                },

                // Progress for "Xây dựng tư duy tích cực"
                new SubscriptionProgress
                {
                    Id = Guid.Parse("b18108d5-6480-4ad0-9d72-7b4c34ec45d0"),
                    Section = 1,
                    Description = "Nhận diện suy nghĩ tiêu cực",
                    Date = 1,
                    SubscriptionId = Guid.Parse("8f436d13-ad84-4761-adb7-2049e907cd2b"),
                    IsCompleted = false,
                    CreateAt = DateTimeOffset.UtcNow
                },
                new SubscriptionProgress
                {
                    Id = Guid.Parse("5ae9b457-a18d-4a67-8397-04e54bb7c8f1"),
                    Section = 2,
                    Description = "Thay đổi góc nhìn về bản thân",
                    Date = 3,
                    SubscriptionId = Guid.Parse("8f436d13-ad84-4761-adb7-2049e907cd2b"),
                    IsCompleted = false,
                    CreateAt = DateTimeOffset.UtcNow
                },
                new SubscriptionProgress
                {
                    Id = Guid.Parse("e6302674-cd73-49c1-9a3c-7fa35600425b"),
                    Section = 3,
                    Description = "Thực hành khẳng định tích cực",
                    Date = 6,
                    SubscriptionId = Guid.Parse("8f436d13-ad84-4761-adb7-2049e907cd2b"),
                    IsCompleted = false,
                    CreateAt = DateTimeOffset.UtcNow
                },
                new SubscriptionProgress
                {
                    Id = Guid.Parse("85bbf6f4-7339-4f30-b347-8b3f2371086c"),
                    Section = 4,
                    Description = "Phát triển tư duy phát triển",
                    Date = 10,
                    SubscriptionId = Guid.Parse("8f436d13-ad84-4761-adb7-2049e907cd2b"),
                    IsCompleted = false,
                    CreateAt = DateTimeOffset.UtcNow
                },
                new SubscriptionProgress
                {
                    Id = Guid.Parse("96b78cef-838a-4177-b5dd-d41293f1d7a8"),
                    Section = 5,
                    Description = "Ứng dụng tư duy tích cực vào cuộc sống",
                    Date = 10,
                    SubscriptionId = Guid.Parse("8f436d13-ad84-4761-adb7-2049e907cd2b"),
                    IsCompleted = false,
                    CreateAt = DateTimeOffset.UtcNow
                },

                // Progress for "Kỹ năng giao tiếp"
                new SubscriptionProgress
                {
                    Id = Guid.Parse("ebcebd5b-e6e0-40f6-9939-cd5aa91727e0"),
                    Section = 1,
                    Description = "Tầm quan trọng của giao tiếp hiệu quả",
                    Date = 1,
                    SubscriptionId = Guid.Parse("4a580bd8-d04c-4980-87f9-456c92ca6471"),
                    IsCompleted = false,
                    CreateAt = DateTimeOffset.UtcNow
                },
                new SubscriptionProgress
                {
                    Id = Guid.Parse("62b29aae-e0f5-49b8-98d0-aa46bb34c08a"),
                    Section = 2,
                    Description = "Lắng nghe tích cực",
                    Date = 3,
                    SubscriptionId = Guid.Parse("4a580bd8-d04c-4980-87f9-456c92ca6471"),
                    IsCompleted = false,
                    CreateAt = DateTimeOffset.UtcNow
                },
                new SubscriptionProgress
                {
                    Id = Guid.Parse("96f10dbc-3931-40d8-826a-b7da64d17c1c"),
                    Section = 3,
                    Description = "Ngôn ngữ cơ thể và giao tiếp phi ngôn ngữ",
                    Date = 6,
                    SubscriptionId = Guid.Parse("4a580bd8-d04c-4980-87f9-456c92ca6471"),
                    IsCompleted = false,
                    CreateAt = DateTimeOffset.UtcNow
                },
                new SubscriptionProgress
                {
                    Id = Guid.Parse("5d871899-8713-42fe-9c05-ef88ebdd5cdb"),
                    Section = 4,
                    Description = "Xây dựng sự tự tin trong giao tiếp",
                    Date = 10,
                    SubscriptionId = Guid.Parse("4a580bd8-d04c-4980-87f9-456c92ca6471"),
                    IsCompleted = false,
                    CreateAt = DateTimeOffset.UtcNow
                },
                new SubscriptionProgress
                {
                    Id = Guid.Parse("bd11496c-7cce-40f3-aa52-3e01ed58c752"),
                    Section = 5,
                    Description = "Ứng dụng kỹ năng giao tiếp vào thực tế",
                    Date = 10,
                    SubscriptionId = Guid.Parse("4a580bd8-d04c-4980-87f9-456c92ca6471"),
                    IsCompleted = false,
                    CreateAt = DateTimeOffset.UtcNow
                },

                // Progress for "Quản lý thời gian hiệu quả"
                new SubscriptionProgress
                {
                    Id = Guid.Parse("c1d2e3f4-a5b6-47c8-9d0e-123456789abc"),
                    Section = 1,
                    Description = "Xác định ưu tiên và mục tiêu",
                    Date = 1,
                    SubscriptionId = Guid.Parse("a1c2f3b4-d5e6-47fa-8bcd-123456789abc"),
                    IsCompleted = false,
                    CreateAt = DateTimeOffset.UtcNow
                },
                new SubscriptionProgress
                {
                    Id = Guid.Parse("d4e5f6a7-b8c9-40da-9ebf-abcdef123456"),
                    Section = 2,
                    Description = "Lập kế hoạch học tập và làm việc",
                    Date = 3,
                    SubscriptionId = Guid.Parse("a1c2f3b4-d5e6-47fa-8bcd-123456789abc"),
                    IsCompleted = false,
                    CreateAt = DateTimeOffset.UtcNow
                },
                new SubscriptionProgress
                {
                    Id = Guid.Parse("e6f7a8b9-c0d1-41eb-af23-456789abcdef"),
                    Section = 3,
                    Description = "Kỹ thuật Pomodoro và ma trận Eisenhower",
                    Date = 6,
                    SubscriptionId = Guid.Parse("a1c2f3b4-d5e6-47fa-8bcd-123456789abc"),
                    IsCompleted = false,
                    CreateAt = DateTimeOffset.UtcNow
                },
                new SubscriptionProgress
                {
                    Id = Guid.Parse("f8a9b0c1-d2e3-42fc-bd34-56789abcdef0"),
                    Section = 4,
                    Description = "Quản lý thời gian trong các tình huống áp lực",
                    Date = 10,
                    SubscriptionId = Guid.Parse("a1c2f3b4-d5e6-47fa-8bcd-123456789abc"),
                    IsCompleted = false,
                    CreateAt = DateTimeOffset.UtcNow
                },
                new SubscriptionProgress
                {
                    Id = Guid.Parse("a0b1c2d3-e4f5-43ad-cd45-6789abcdef01"),
                    Section = 5,
                    Description = "Tối ưu hóa lịch trình cá nhân",
                    Date = 12,
                    SubscriptionId = Guid.Parse("a1c2f3b4-d5e6-47fa-8bcd-123456789abc"),
                    IsCompleted = false,
                    CreateAt = DateTimeOffset.UtcNow
                },

                // Progress for "Giảm căng thẳng và lo âu"
                new SubscriptionProgress
                {
                    Id = Guid.Parse("b1c2d3e4-f5a6-44bc-de56-123456789abc"),
                    Section = 1,
                    Description = "Nhận diện các dấu hiệu căng thẳng",
                    Date = 1,
                    SubscriptionId = Guid.Parse("f4e3d2c1-b6a7-48fb-9cde-abcdef123456"),
                    IsCompleted = false,
                    CreateAt = DateTimeOffset.UtcNow
                },
                new SubscriptionProgress
                {
                    Id = Guid.Parse("c3d4e5f6-a7b8-45de-ef67-abcdef123456"),
                    Section = 2,
                    Description = "Áp dụng kỹ thuật hít thở sâu",
                    Date = 3,
                    SubscriptionId = Guid.Parse("f4e3d2c1-b6a7-48fb-9cde-abcdef123456"),
                    IsCompleted = false,
                    CreateAt = DateTimeOffset.UtcNow
                },
                new SubscriptionProgress
                {
                    Id = Guid.Parse("d5e6f7a8-b9c0-46ef-af78-456789abcdef"),
                    Section = 3,
                    Description = "Thực hành thiền và yoga giảm stress",
                    Date = 6,
                    SubscriptionId = Guid.Parse("f4e3d2c1-b6a7-48fb-9cde-abcdef123456"),
                    IsCompleted = false,
                    CreateAt = DateTimeOffset.UtcNow
                },
                new SubscriptionProgress
                {
                    Id = Guid.Parse("e7f8a9b0-c1d2-47fd-bc89-56789abcdef0"),
                    Section = 4,
                    Description = "Phát triển tư duy tích cực để giảm lo âu",
                    Date = 10,
                    SubscriptionId = Guid.Parse("f4e3d2c1-b6a7-48fb-9cde-abcdef123456"),
                    IsCompleted = false,
                    CreateAt = DateTimeOffset.UtcNow
                },
                new SubscriptionProgress
                {
                    Id = Guid.Parse("f9a0b1c2-d3e4-48ed-df90-6789abcdef01"),
                    Section = 5,
                    Description = "Ứng dụng kỹ năng quản lý căng thẳng vào cuộc sống",
                    Date = 12,
                    SubscriptionId = Guid.Parse("f4e3d2c1-b6a7-48fb-9cde-abcdef123456"),
                    IsCompleted = false,
                    CreateAt = DateTimeOffset.UtcNow
                }
            );

            //Account-survey(m-m: AccountSurvey)
            modelBuilder.Entity<Psychologist>()
           .HasOne(p => p.Account)
           .WithOne(a => a.Psychologist)
           .HasForeignKey<Psychologist>(p => p.AccountId)
           .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Account>().HasMany(s => s.AccountSurveys).WithOne(a => a.Account).HasForeignKey(s => s.AccountId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Survey>().HasMany(s => s.AccountSurveys).WithOne(a => a.Survey).HasForeignKey(s => s.SurveyId).OnDelete(DeleteBehavior.Restrict);

            //Survey-SurveyResults(1-m)
            modelBuilder.Entity<Survey>().HasMany(s => s.SurveyResults).WithOne(a => a.Survey).HasForeignKey(s => s.SurveyId).OnDelete(DeleteBehavior.Restrict);

            //SurveyType-Survey(1-m)
            modelBuilder.Entity<SurveyType>().HasMany(s => s.Surveys).WithOne(a => a.SurveyType).HasForeignKey(s => s.SurveyTypeId).OnDelete(DeleteBehavior.Restrict);

            //SurveyType-SurveyQuestion(1-m)
            modelBuilder.Entity<SurveyType>().HasMany(s => s.SurveyQuestions).WithOne(a => a.SurveyType).HasForeignKey(s => s.SurveyTypeId).OnDelete(DeleteBehavior.Restrict);

            //account - appointment
            modelBuilder.Entity<Account>().HasMany(t => t.Appointments).WithOne(a => a.Account).HasForeignKey(t => t.AccountId).OnDelete(DeleteBehavior.Restrict);

            //Psychologist - appointment
            modelBuilder.Entity<Psychologist>().HasMany(t => t.Appointments).WithOne(a => a.Psychologist).HasForeignKey(t => t.PsychologistId).OnDelete(DeleteBehavior.Restrict);

            //account - Psychologist(m-m)
            modelBuilder.Entity<Account>().HasMany(s => s.HealthDatas).WithOne(a => a.Account).HasForeignKey(s => s.AccountId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Psychologist>().HasMany(s => s.HealthDatas).WithOne(a => a.Psychologist).HasForeignKey(s => s.PsychologistId).OnDelete(DeleteBehavior.Restrict);

            //Psychologist - SubscriptionData(1 -n)
            modelBuilder.Entity<Psychologist>().HasMany(s => s.SubscriptionDatas).WithOne(a => a.Psychologists).HasForeignKey(s => s.PsychologistId).OnDelete(DeleteBehavior.Restrict);

            //order - transaction(1-m)
            modelBuilder.Entity<Order>().HasMany(o => o.Transactions).WithOne(t => t.Order).HasForeignKey(o => o.OrderId).OnDelete(DeleteBehavior.Restrict);

            //order - subscription(m-1)
            modelBuilder.Entity<SubscriptionData>().HasMany(o => o.Orders).WithOne(d => d.SubscriptionData).HasForeignKey(o => o.SubscriptionDataId).OnDelete(DeleteBehavior.Restrict);

            //order - account(m-1)
            modelBuilder.Entity<Account>().HasMany(o => o.Orders).WithOne(d => d.Accounts).HasForeignKey(o => o.AccountId).OnDelete(DeleteBehavior.Restrict);

            //Category - SubscriptionData(1-m)
            modelBuilder.Entity<Category>().HasMany(d => d.SubscriptionDatas).WithOne(c => c.Category).HasForeignKey(d => d.CategoryId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SurveyQuestionSurvey>()
                        .Ignore(e => e.Id) // Ignore Id property during insert
                        .Ignore(e => e.IsDeleted); // Ignore IsDeleted property during insert

            modelBuilder.Entity<SurveyQuestionAnswer>()
            .Ignore(e => e.Id) // Ignore Id property during insert
            .Ignore(e => e.IsDeleted); // Ignore IsDeleted property during insert

            modelBuilder.Entity<SurveyQuestionAnswer>()
                .Property(s => s.SurveyQuestionsId)
                .HasColumnName("SurveyQuestionsId");

            modelBuilder.Entity<SurveyQuestionAnswer>()
            .HasKey(sq => new { sq.SurveyAnswersId, sq.SurveyQuestionsId });

            modelBuilder.Entity<SurveyQuestionSurvey>()
                .HasKey(sq => new { sq.SurveyQuestionsId, sq.SurveysId });

            modelBuilder.Entity<SurveyQuestionAnswer>()
                .HasOne(sqa => sqa.SurveyAnswer)
                .WithMany(sa => sa.SurveyQuestionAnswers)
                .HasForeignKey(sqa => sqa.SurveyAnswersId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SurveyQuestionAnswer>()
                .HasOne(sqa => sqa.SurveyQuestion)
                .WithMany(sq => sq.SurveyQuestionAnswers)
                .HasForeignKey(sqa => sqa.SurveyQuestionsId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SurveyQuestion>()
                .HasOne(sq => sq.SurveyType)
                .WithMany(st => st.SurveyQuestions)
                .HasForeignKey(sq => sq.SurveyTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SurveyQuestionSurvey>()
                .HasOne(sqs => sqs.SurveyQuestion)
                .WithMany(sq => sq.SurveyQuestionSurveys)
                .HasForeignKey(sqs => sqs.SurveyQuestionsId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SurveyQuestionSurvey>()
                .HasOne(sqs => sqs.Survey)
                .WithMany(s => s.SurveyQuestionSurveys)
                .HasForeignKey(sqs => sqs.SurveysId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SurveyQuestionSurvey>().HasData(
                new SurveyQuestionSurvey { 
                    SurveyQuestionsId = Guid.Parse("0998C44C-7A8D-41FD-8B3E-05D775CE4B27"), 
                    SurveysId = Guid.Parse("418BE23D-8DB9-4B74-A86A-9402F246EA62") },
                new SurveyQuestionSurvey
                {
                    SurveyQuestionsId = Guid.Parse("7B1CC80A-8C8F-4695-99EB-271C46B9C766"),
                    SurveysId = Guid.Parse("418BE23D-8DB9-4B74-A86A-9402F246EA62")
                },
                new SurveyQuestionSurvey
                {
                    SurveyQuestionsId = Guid.Parse("07CB8575-7B7D-4784-A6B2-9CC1543F0267"),
                    SurveysId = Guid.Parse("418BE23D-8DB9-4B74-A86A-9402F246EA62")
                },
                new SurveyQuestionSurvey
                {
                    SurveyQuestionsId = Guid.Parse("A9FFEDD5-3F8E-4D98-B230-AE2491045F0D"),
                    SurveysId = Guid.Parse("418BE23D-8DB9-4B74-A86A-9402F246EA62")
                },
                new SurveyQuestionSurvey
                {
                    SurveyQuestionsId = Guid.Parse("4F4736E4-ECBE-4BDC-A9CF-CAC7DB918AAE"),
                    SurveysId = Guid.Parse("418BE23D-8DB9-4B74-A86A-9402F246EA62")
                },
                new SurveyQuestionSurvey
                {
                    SurveyQuestionsId = Guid.Parse("798B80AC-F766-4BAF-AA01-DBEBCA21F98B"),
                    SurveysId = Guid.Parse("418BE23D-8DB9-4B74-A86A-9402F246EA62")
                },
                new SurveyQuestionSurvey
                {
                    SurveyQuestionsId = Guid.Parse("A1CE2D59-88D4-463D-8AF2-F472B5771746"),
                    SurveysId = Guid.Parse("418BE23D-8DB9-4B74-A86A-9402F246EA62")
                },

                new SurveyQuestionSurvey
                {
                    SurveyQuestionsId = Guid.Parse("37A557B5-C10A-4062-B7E5-0B2F4E02636E"),
                    SurveysId = Guid.Parse("86DEEB52-2EF9-47D9-8496-EDAC723FFBF7")
                },
                new SurveyQuestionSurvey
                {
                    SurveyQuestionsId = Guid.Parse("59D5F97C-11B1-40D3-A69C-6C3700FF2C7D"),
                    SurveysId = Guid.Parse("86DEEB52-2EF9-47D9-8496-EDAC723FFBF7")
                },
                new SurveyQuestionSurvey
                {
                    SurveyQuestionsId = Guid.Parse("FDCE1E6A-D8DC-4DDC-BD63-9F9ACF666467"),
                    SurveysId = Guid.Parse("86DEEB52-2EF9-47D9-8496-EDAC723FFBF7")
                },
                new SurveyQuestionSurvey
                {
                    SurveyQuestionsId = Guid.Parse("A36264C0-9AD8-4205-811B-A867D2C7966D"),
                    SurveysId = Guid.Parse("86DEEB52-2EF9-47D9-8496-EDAC723FFBF7")
                },
                new SurveyQuestionSurvey
                {
                    SurveyQuestionsId = Guid.Parse("416FCDF0-E6E6-4E76-9E3A-CB80B9A57A49"),
                    SurveysId = Guid.Parse("86DEEB52-2EF9-47D9-8496-EDAC723FFBF7")
                },
                new SurveyQuestionSurvey
                {
                    SurveyQuestionsId = Guid.Parse("3F0DCE08-EBBD-4872-B9DC-D3DE0B66279F"),
                    SurveysId = Guid.Parse("86DEEB52-2EF9-47D9-8496-EDAC723FFBF7")
                },
                new SurveyQuestionSurvey
                {
                    SurveyQuestionsId = Guid.Parse("8F0A5958-04AD-403A-BBAD-EA476F5C2CC5"),
                    SurveysId = Guid.Parse("86DEEB52-2EF9-47D9-8496-EDAC723FFBF7")
                },
                new SurveyQuestionSurvey
                {
                    SurveyQuestionsId = Guid.Parse("41ECF897-7BFA-4892-ABB8-EEE9AACC7EB5"),
                    SurveysId = Guid.Parse("86DEEB52-2EF9-47D9-8496-EDAC723FFBF7")
                },
                new SurveyQuestionSurvey
                {
                    SurveyQuestionsId = Guid.Parse("0FF7226B-02F5-45DD-B3A6-F3BB2C804C5D"),
                    SurveysId = Guid.Parse("86DEEB52-2EF9-47D9-8496-EDAC723FFBF7")
                }
    );

            modelBuilder.Entity<SurveyQuestionAnswer>().HasData(
                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("28dee921-db63-45c3-87a3-bdf0a263370d"),
                    SurveyQuestionsId = Guid.Parse("0998C44C-7A8D-41FD-8B3E-05D775CE4B27")
                },
                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("66d927b9-e3c2-47f3-8aa9-82260d1579cd"),
                    SurveyQuestionsId = Guid.Parse("0998C44C-7A8D-41FD-8B3E-05D775CE4B27")
                },
                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("91c14006-832a-41dd-8d03-e7950cf54347"),
                    SurveyQuestionsId = Guid.Parse("0998C44C-7A8D-41FD-8B3E-05D775CE4B27")
                },
                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("f7095907-611f-4ed9-b622-5df984f48175"),
                    SurveyQuestionsId = Guid.Parse("0998C44C-7A8D-41FD-8B3E-05D775CE4B27")
                },
                ////////////////////////////////////
                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("28DEE921-DB63-45C3-87A3-BDF0A263370D"),
                    SurveyQuestionsId = Guid.Parse("37A557B5-C10A-4062-B7E5-0B2F4E02636E")
                },
                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("66D927B9-E3C2-47F3-8AA9-82260D1579CD"),
                    SurveyQuestionsId = Guid.Parse("37A557B5-C10A-4062-B7E5-0B2F4E02636E")
                },
                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("91C14006-832A-41DD-8D03-E7950CF54347"),
                    SurveyQuestionsId = Guid.Parse("37A557B5-C10A-4062-B7E5-0B2F4E02636E")
                },
                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("F7095907-611F-4ED9-B622-5DF984F48175"),
                    SurveyQuestionsId = Guid.Parse("37A557B5-C10A-4062-B7E5-0B2F4E02636E")
                },
                //////////////////////////
                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("28DEE921-DB63-45C3-87A3-BDF0A263370D"),
                    SurveyQuestionsId = Guid.Parse("7B1CC80A-8C8F-4695-99EB-271C46B9C766")
                },
                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("66D927B9-E3C2-47F3-8AA9-82260D1579CD"),
                    SurveyQuestionsId = Guid.Parse("7B1CC80A-8C8F-4695-99EB-271C46B9C766")
                },
                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("91C14006-832A-41DD-8D03-E7950CF54347"),
                    SurveyQuestionsId = Guid.Parse("7B1CC80A-8C8F-4695-99EB-271C46B9C766")
                },
                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("F7095907-611F-4ED9-B622-5DF984F48175"),
                    SurveyQuestionsId = Guid.Parse("7B1CC80A-8C8F-4695-99EB-271C46B9C766")
                },
                ///////////////////////////////
                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("F7095907-611F-4ED9-B622-5DF984F48175"),
                    SurveyQuestionsId = Guid.Parse("59D5F97C-11B1-40D3-A69C-6C3700FF2C7D")
                },
                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("66D927B9-E3C2-47F3-8AA9-82260D1579CD"),
                    SurveyQuestionsId = Guid.Parse("59D5F97C-11B1-40D3-A69C-6C3700FF2C7D")
                },
                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("28DEE921-DB63-45C3-87A3-BDF0A263370D"),
                    SurveyQuestionsId = Guid.Parse("59D5F97C-11B1-40D3-A69C-6C3700FF2C7D")
                },
                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("91C14006-832A-41DD-8D03-E7950CF54347"),
                    SurveyQuestionsId = Guid.Parse("59D5F97C-11B1-40D3-A69C-6C3700FF2C7D")
                },
                ////////////////////////
                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("28DEE921-DB63-45C3-87A3-BDF0A263370D"),
                    SurveyQuestionsId = Guid.Parse("07CB8575-7B7D-4784-A6B2-9CC1543F0267")
                },
                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("66D927B9-E3C2-47F3-8AA9-82260D1579CD"),
                    SurveyQuestionsId = Guid.Parse("07CB8575-7B7D-4784-A6B2-9CC1543F0267")
                },
                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("91C14006-832A-41DD-8D03-E7950CF54347"),
                    SurveyQuestionsId = Guid.Parse("07CB8575-7B7D-4784-A6B2-9CC1543F0267")
                },
                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("F7095907-611F-4ED9-B622-5DF984F48175"),
                    SurveyQuestionsId = Guid.Parse("07CB8575-7B7D-4784-A6B2-9CC1543F0267")
                },
                //////////////////////
                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("28DEE921-DB63-45C3-87A3-BDF0A263370D"),
                    SurveyQuestionsId = Guid.Parse("FDCE1E6A-D8DC-4DDC-BD63-9F9ACF666467")
                },
                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("66D927B9-E3C2-47F3-8AA9-82260D1579CD"),
                    SurveyQuestionsId = Guid.Parse("FDCE1E6A-D8DC-4DDC-BD63-9F9ACF666467")
                },
                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("91C14006-832A-41DD-8D03-E7950CF54347"),
                    SurveyQuestionsId = Guid.Parse("FDCE1E6A-D8DC-4DDC-BD63-9F9ACF666467")
                },
                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("F7095907-611F-4ED9-B622-5DF984F48175"),
                    SurveyQuestionsId = Guid.Parse("FDCE1E6A-D8DC-4DDC-BD63-9F9ACF666467")
                },
                /////////////////////////////
                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("28DEE921-DB63-45C3-87A3-BDF0A263370D"),
                    SurveyQuestionsId = Guid.Parse("A36264C0-9AD8-4205-811B-A867D2C7966D")
                },
                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("66D927B9-E3C2-47F3-8AA9-82260D1579CD"),
                    SurveyQuestionsId = Guid.Parse("A36264C0-9AD8-4205-811B-A867D2C7966D")
                },
                
                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("91C14006-832A-41DD-8D03-E7950CF54347"),
                    SurveyQuestionsId = Guid.Parse("A36264C0-9AD8-4205-811B-A867D2C7966D")
                },
                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("F7095907-611F-4ED9-B622-5DF984F48175"),
                    SurveyQuestionsId = Guid.Parse("A36264C0-9AD8-4205-811B-A867D2C7966D")
                },
                //////////////////////
                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("28DEE921-DB63-45C3-87A3-BDF0A263370D"),
                    SurveyQuestionsId = Guid.Parse("A9FFEDD5-3F8E-4D98-B230-AE2491045F0D")
                },            
                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("66D927B9-E3C2-47F3-8AA9-82260D1579CD"),
                    SurveyQuestionsId = Guid.Parse("A9FFEDD5-3F8E-4D98-B230-AE2491045F0D")
                },
                
                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("91C14006-832A-41DD-8D03-E7950CF54347"),
                    SurveyQuestionsId = Guid.Parse("A9FFEDD5-3F8E-4D98-B230-AE2491045F0D")
                },
                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("F7095907-611F-4ED9-B622-5DF984F48175"),
                    SurveyQuestionsId = Guid.Parse("A9FFEDD5-3F8E-4D98-B230-AE2491045F0D")
                },
                ////////////////////////////
                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("28DEE921-DB63-45C3-87A3-BDF0A263370D"),
                    SurveyQuestionsId = Guid.Parse("4F4736E4-ECBE-4BDC-A9CF-CAC7DB918AAE")
                },               
                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("66D927B9-E3C2-47F3-8AA9-82260D1579CD"),
                    SurveyQuestionsId = Guid.Parse("4F4736E4-ECBE-4BDC-A9CF-CAC7DB918AAE")
                },
                
                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("91C14006-832A-41DD-8D03-E7950CF54347"),
                    SurveyQuestionsId = Guid.Parse("4F4736E4-ECBE-4BDC-A9CF-CAC7DB918AAE")
                },
                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("F7095907-611F-4ED9-B622-5DF984F48175"),
                    SurveyQuestionsId = Guid.Parse("4F4736E4-ECBE-4BDC-A9CF-CAC7DB918AAE")
                },
                ///////////////////////////

                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("28DEE921-DB63-45C3-87A3-BDF0A263370D"),
                    SurveyQuestionsId = Guid.Parse("416FCDF0-E6E6-4E76-9E3A-CB80B9A57A49")
                },
                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("66D927B9-E3C2-47F3-8AA9-82260D1579CD"),
                    SurveyQuestionsId = Guid.Parse("416FCDF0-E6E6-4E76-9E3A-CB80B9A57A49")
                },
                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("91C14006-832A-41DD-8D03-E7950CF54347"),
                    SurveyQuestionsId = Guid.Parse("416FCDF0-E6E6-4E76-9E3A-CB80B9A57A49")
                },
                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("F7095907-611F-4ED9-B622-5DF984F48175"),
                    SurveyQuestionsId = Guid.Parse("416FCDF0-E6E6-4E76-9E3A-CB80B9A57A49")
                },
                /////////////////////////

                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("28DEE921-DB63-45C3-87A3-BDF0A263370D"),
                    SurveyQuestionsId = Guid.Parse("3F0DCE08-EBBD-4872-B9DC-D3DE0B66279F")
                },
                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("66D927B9-E3C2-47F3-8AA9-82260D1579CD"),
                    SurveyQuestionsId = Guid.Parse("3F0DCE08-EBBD-4872-B9DC-D3DE0B66279F")
                },
                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("91C14006-832A-41DD-8D03-E7950CF54347"),
                    SurveyQuestionsId = Guid.Parse("3F0DCE08-EBBD-4872-B9DC-D3DE0B66279F")
                },
                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("F7095907-611F-4ED9-B622-5DF984F48175"),
                    SurveyQuestionsId = Guid.Parse("3F0DCE08-EBBD-4872-B9DC-D3DE0B66279F")
                },
                ///////////////////////////////////
                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("28DEE921-DB63-45C3-87A3-BDF0A263370D"),
                    SurveyQuestionsId = Guid.Parse("798B80AC-F766-4BAF-AA01-DBEBCA21F98B")
                },
                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("66D927B9-E3C2-47F3-8AA9-82260D1579CD"),
                    SurveyQuestionsId = Guid.Parse("798B80AC-F766-4BAF-AA01-DBEBCA21F98B")
                },
                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("91C14006-832A-41DD-8D03-E7950CF54347"),
                    SurveyQuestionsId = Guid.Parse("798B80AC-F766-4BAF-AA01-DBEBCA21F98B")
                },
                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("F7095907-611F-4ED9-B622-5DF984F48175"),
                    SurveyQuestionsId = Guid.Parse("798B80AC-F766-4BAF-AA01-DBEBCA21F98B")
                },
                /////////////////////////////

                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("28DEE921-DB63-45C3-87A3-BDF0A263370D"),
                    SurveyQuestionsId = Guid.Parse("8F0A5958-04AD-403A-BBAD-EA476F5C2CC5")
                },
                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("66D927B9-E3C2-47F3-8AA9-82260D1579CD"),
                    SurveyQuestionsId = Guid.Parse("8F0A5958-04AD-403A-BBAD-EA476F5C2CC5")
                },
                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("91C14006-832A-41DD-8D03-E7950CF54347"),
                    SurveyQuestionsId = Guid.Parse("8F0A5958-04AD-403A-BBAD-EA476F5C2CC5")
                },
                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("F7095907-611F-4ED9-B622-5DF984F48175"),
                    SurveyQuestionsId = Guid.Parse("8F0A5958-04AD-403A-BBAD-EA476F5C2CC5")
                },
                /////////////////////////

                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("28DEE921-DB63-45C3-87A3-BDF0A263370D"),
                    SurveyQuestionsId = Guid.Parse("41ECF897-7BFA-4892-ABB8-EEE9AACC7EB5")
                },
                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("66D927B9-E3C2-47F3-8AA9-82260D1579CD"),
                    SurveyQuestionsId = Guid.Parse("41ECF897-7BFA-4892-ABB8-EEE9AACC7EB5")
                },
                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("91C14006-832A-41DD-8D03-E7950CF54347"),
                    SurveyQuestionsId = Guid.Parse("41ECF897-7BFA-4892-ABB8-EEE9AACC7EB5")
                },
                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("F7095907-611F-4ED9-B622-5DF984F48175"),
                    SurveyQuestionsId = Guid.Parse("41ECF897-7BFA-4892-ABB8-EEE9AACC7EB5")
                },
                //////////////////////////////////

                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("28DEE921-DB63-45C3-87A3-BDF0A263370D"),
                    SurveyQuestionsId = Guid.Parse("0FF7226B-02F5-45DD-B3A6-F3BB2C804C5D")
                },
                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("66D927B9-E3C2-47F3-8AA9-82260D1579CD"),
                    SurveyQuestionsId = Guid.Parse("0FF7226B-02F5-45DD-B3A6-F3BB2C804C5D")
                },
                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("91C14006-832A-41DD-8D03-E7950CF54347"),
                    SurveyQuestionsId = Guid.Parse("0FF7226B-02F5-45DD-B3A6-F3BB2C804C5D")
                },
                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("F7095907-611F-4ED9-B622-5DF984F48175"),
                    SurveyQuestionsId = Guid.Parse("0FF7226B-02F5-45DD-B3A6-F3BB2C804C5D")
                },
                /////////////////////////////

                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("28DEE921-DB63-45C3-87A3-BDF0A263370D"),
                    SurveyQuestionsId = Guid.Parse("A1CE2D59-88D4-463D-8AF2-F472B5771746")
                },
                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("66D927B9-E3C2-47F3-8AA9-82260D1579CD"),
                    SurveyQuestionsId = Guid.Parse("A1CE2D59-88D4-463D-8AF2-F472B5771746")
                },
                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("91C14006-832A-41DD-8D03-E7950CF54347"),
                    SurveyQuestionsId = Guid.Parse("A1CE2D59-88D4-463D-8AF2-F472B5771746")
                },
                new SurveyQuestionAnswer
                {
                    SurveyAnswersId = Guid.Parse("F7095907-611F-4ED9-B622-5DF984F48175"),
                    SurveyQuestionsId = Guid.Parse("A1CE2D59-88D4-463D-8AF2-F472B5771746")
                }
            );
            // SubscriptionData - SubscriptionProgress (1-m)
            modelBuilder.Entity<SubscriptionData>().HasMany(p => p.SubscriptionProgresses).WithOne(d => d.SubscriptionDatas).HasForeignKey(p => p.SubscriptionId).OnDelete(DeleteBehavior.Restrict);
            // SubscriptionData - UserProgress (1-m)
            modelBuilder.Entity<SubscriptionData>().HasMany(up => up.UserProgresses).WithOne(d => d.SubscriptionData).HasForeignKey(up => up.SubscriptionId).OnDelete(DeleteBehavior.Restrict);
            //Account - UserProgress (1-m)
            modelBuilder.Entity<Account>().HasMany(up => up.UserProgresses).WithOne(a => a.Accounts).HasForeignKey(up => up.AccountId).OnDelete(DeleteBehavior.Restrict);
        }

    }
}
