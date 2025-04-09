using HEALTH_SUPPORT.Repositories;
using Microsoft.EntityFrameworkCore;
using HEALTH_SUPPORT.Repositories.Entities;
using HEALTH_SUPPORT.Repositories.Repository;
using HEALTH_SUPPORT.Services.IServices;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Services.Implementations
{
    public class AppointmentReminderServiceImpl : IAppointmentReminderService
    {
        private readonly IBaseRepository<Appointment, Guid> _appointmentRepository;
        private readonly IEmailService _emailService;

        public AppointmentReminderServiceImpl(
            IBaseRepository<Appointment, Guid> appointmentRepository,
            IEmailService emailService)
        {
            _appointmentRepository = appointmentRepository;
            _emailService = emailService;
        }

        public async Task CheckAndSendRemindersAsync()
        {
            var now = DateTime.UtcNow;
            var in25Min = now.AddMinutes(25);
            var in30Min = now.AddMinutes(30);

            // Dùng GetAll + Include thông qua IQueryable
            var appointments = await _appointmentRepository.GetAll()
                .Where(a => !a.IsDeleted &&
                            a.AppointmentDate >= in25Min &&
                            a.AppointmentDate <= in30Min)
                .Include(a => a.Account)
                .Include(a => a.Psychologist)
                .ToListAsync();

            foreach (var appointment in appointments)
            {
                var account = appointment.Account;
                var psychologist = appointment.Psychologist;

                if (account != null && psychologist != null)
                {
                    _emailService.SendAppointmentReminder(
                        account.Email,
                        account.Fullname,
                        appointment.AppointmentDate,
                        psychologist.UrlMeet
                    );
                }
            }
        }
    }
}
