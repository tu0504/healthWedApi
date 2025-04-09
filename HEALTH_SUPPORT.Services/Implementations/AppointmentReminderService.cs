using HEALTH_SUPPORT.Services.IServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Services.Implementations
{
    public class AppointmentReminderService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public AppointmentReminderService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var reminderService = scope.ServiceProvider.GetRequiredService<IAppointmentReminderService>();
                    await reminderService.CheckAndSendRemindersAsync();
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); // ví dụ check mỗi phút
            }
        }
    }
}