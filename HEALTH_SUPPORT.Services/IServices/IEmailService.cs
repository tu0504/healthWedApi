using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Services.IServices
{
    public interface IEmailService
    {
        void GenerateOtp(string email);
        bool VerifyOtp(string email, string otp);
        void SendAppointmentReminder(string to, string accountName, DateTimeOffset appointmentTime, string meetLink);


    }
}
