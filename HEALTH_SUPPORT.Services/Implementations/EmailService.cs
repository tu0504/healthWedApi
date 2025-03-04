using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using HEALTH_SUPPORT.Services.IServices;
using HEALTH_SUPPORT.Repositories.Repository;
using HEALTH_SUPPORT.Repositories.Entities;

namespace HEALTH_SUPPORT.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ConcurrentDictionary<string, (string Otp, DateTime ExpiresAt)> _otpCache = new();

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void GenerateOtp(string email)
        {
            var otp = new Random().Next(100000, 999999).ToString();
            var expiresAt = DateTime.UtcNow.AddMinutes(5);
            _otpCache[email] = (otp, expiresAt);

            SendEmail(email, "Mã OTP của bạn", $"Mã OTP của bạn là: {otp}");
        }

        private void SendEmail(string to, string subject, string body)
        {
            var smtpClient = new SmtpClient(_configuration["EmailSettings:SmtpServer"])
            {
                Port = int.Parse(_configuration["EmailSettings:Port"]),
                Credentials = new NetworkCredential(_configuration["EmailSettings:Username"], _configuration["EmailSettings:Password"]),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration["EmailSettings:FromEmail"]),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(to);

            smtpClient.Send(mailMessage);
        }

        public bool VerifyOtp(string email, string otp)
        {
            if (!_otpCache.TryGetValue(email, out var otpData))
            {
                Console.WriteLine($"[DEBUG] OTP không tồn tại cho email: {email}");
                return false;
            }

            var (storedOtp, expiresAt) = otpData;

            if (DateTime.UtcNow > expiresAt)
            {
                Console.WriteLine($"[DEBUG] OTP đã hết hạn cho email: {email}");
                _otpCache.TryRemove(email, out _); // Xóa OTP hết hạn
                return false;
            }

            if (!storedOtp.Equals(otp.Trim()))
            {
                Console.WriteLine($"[DEBUG] OTP không đúng: Nhập '{otp.Trim()}', Lưu '{storedOtp}'");
                return false;
            }

            _otpCache.TryRemove(email, out _); // Xác thực xong thì xóa OTP để tránh sử dụng lại

            return true;
        }
    }
}
