
using HEALTH_SUPPORT.Repositories;
using HEALTH_SUPPORT.Repositories.Repository;
using HEALTH_SUPPORT.Services.Implementations;
using HEALTH_SUPPORT.Services.IServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

namespace HEALTH_SUPPORT.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. Đọc chuỗi cấu hình JWT từ appsettings.json
            var jwtSettings = builder.Configuration.GetSection("JwtSettings");
            string secretKey = jwtSettings["SecretKey"];
            string issuer = jwtSettings["Issuer"];
            string audience = jwtSettings["Audience"];

            // 2. Đăng ký Authentication với JwtBearer
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false; // để dev local cho nhanh, prod thì nên để true
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateLifetime = true, // kiểm tra token hết hạn
                    ClockSkew = TimeSpan.Zero
                };
            });

            // 3. Thêm Authorization
            builder.Services.AddAuthorization();

            // 4. Thêm các service khác (Repositories, Services, v.v.)
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add services to the container.
            builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Register ApplicationDbContext
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Register IBaseRepository and BaseRepository
            builder.Services.AddScoped(typeof(IBaseRepository<,>), typeof(BaseRepository<,>));
            // Register IAccountService and AccountService
            builder.Services.AddScoped<IAccountService, AccountService>();
            // Register ISubscriptionService and SubscriptionService
            builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
