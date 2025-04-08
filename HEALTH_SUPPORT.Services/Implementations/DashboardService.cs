using HEALTH_SUPPORT.Repositories;
using HEALTH_SUPPORT.Repositories.Entities;
using HEALTH_SUPPORT.Repositories.Repository;
using HEALTH_SUPPORT.Services.IServices;
using HEALTH_SUPPORT.Services.ResponseModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using static HEALTH_SUPPORT.Services.ResponseModel.DashboardResponse;

namespace HEALTH_SUPPORT.Services.Implementations
{
    public class DashboardService : IDashboardService
    {
        private readonly IBaseRepository<Order, Guid> _orderRepository;
        private readonly IBaseRepository<Transaction, Guid> _transactionRepository;
        private readonly ILogger<DashboardService> _logger;
        private readonly IBaseRepository<Appointment, Guid> _appointmentRepository;
        private readonly IBaseRepository<AccountSurvey, Guid> _accountSurveyRepository;
        private readonly IBaseRepository<Account, Guid> _accountRepository;

        public DashboardService(IBaseRepository<Order, Guid> orderRepository, IBaseRepository<Transaction, Guid> transactionRepository, ILogger<DashboardService> logger, IBaseRepository<Appointment, Guid> appointmentRepository, IBaseRepository<AccountSurvey, Guid> accountSurveyRepository, IBaseRepository<Account, Guid> accountRepository)
        {
            _orderRepository = orderRepository;
            _transactionRepository = transactionRepository;
            _logger = logger;
            _appointmentRepository = appointmentRepository;
            _accountSurveyRepository = accountSurveyRepository;
            _accountRepository = accountRepository;
        }

        public async Task<DashboardResponse.DashboardStats> GetDashboardStats()
        {
            try
            {
                var currentYear = DateTime.Now.Year;
                var stats = new DashboardResponse.DashboardStats
                {
                    Year = currentYear,
                    MonthlyRevenue = new Dictionary<int, float>(),
                    MonthlySubscriptions = new Dictionary<int, Dictionary<string, int>>()
                };

                // Initialize all months with zero revenue and empty subscription types
                var subscriptionTypes = new[] { "Kiểm soát cảm xúc", "Xây dựng tư duy tích cực", 
                    "Kỹ năng giao tiếp", "Quản lý thời gian hiệu quả", "Giảm căng thẳng và lo âu" };

                for (int month = 1; month <= 12; month++)
                {
                    stats.MonthlyRevenue[month] = 0f;
                    stats.MonthlySubscriptions[month] = subscriptionTypes.ToDictionary(type => type, type => 0);
                }

                // Get revenue data
                var transactions = await _transactionRepository.GetAll()
                    .Where(t => t.PaymentStatus == "success" && !t.IsDeleted && t.CreateAt.Year == currentYear)
                    .ToListAsync();

                var revenueByMonth = transactions
                    .GroupBy(t => t.CreateAt.Month)
                    .Select(g => new { Month = g.Key, Total = (float)g.Sum(t => t.Amount) });

                foreach (var revenue in revenueByMonth)
                {
                    stats.MonthlyRevenue[revenue.Month] = revenue.Total;
                }

                // Get subscription data
                var subscriptions = await _orderRepository.GetAll()
                    .Include(o => o.SubscriptionData)
                    .Where(o => o.IsSuccessful && !o.IsDeleted && o.CreateAt.Year == currentYear)
                    .ToListAsync();

                var subscriptionsByMonth = subscriptions
                    .GroupBy(o => new { Month = o.CreateAt.Month, Type = o.SubscriptionData.SubscriptionName })
                    .Select(g => new { 
                        Month = g.Key.Month, 
                        Type = g.Key.Type, 
                        Count = g.Count() 
                    });

                foreach (var subscription in subscriptionsByMonth)
                {
                    if (stats.MonthlySubscriptions.ContainsKey(subscription.Month) &&
                        stats.MonthlySubscriptions[subscription.Month].ContainsKey(subscription.Type))
                    {
                        stats.MonthlySubscriptions[subscription.Month][subscription.Type] = subscription.Count;
                    }
                }

                _logger.LogInformation("Retrieved dashboard stats for year {Year}", currentYear);
                return stats;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting dashboard stats");
                throw;
            }
        }

        public async Task<List<DashboardResponse.MonthlySubscriptionStats>> GetMonthlySubscriptionStats()
        {
            try
            {
                var currentYear = DateTime.Now.Year;
                var stats = await _orderRepository.GetAll()
                    .Where(o => o.IsSuccessful && !o.IsDeleted && o.CreateAt.Year == currentYear)
                    .GroupBy(o => new { Month = o.CreateAt.Month })
                    .Select(g => new DashboardResponse.MonthlySubscriptionStats
                    {
                        Month = g.Key.Month,
                        MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key.Month),
                        TotalSubscriptions = g.Count()
                    })
                    .OrderBy(s => s.Month)
                    .ToListAsync();

                _logger.LogInformation("Retrieved monthly subscription stats for year {Year}", currentYear);
                return stats;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting monthly subscription stats");
                throw;
            }
        }

        public async Task<List<DashboardResponse.MonthlyRevenueStats>> GetMonthlyRevenueStats()
        {
            try
            {
                var transactions = await _transactionRepository.GetAll()
                    .Where(t => t.PaymentStatus == "success" && !t.IsDeleted)
                    .ToListAsync();

                var groupedData = transactions
                    .GroupBy(t => new { t.CreateAt.Year, t.CreateAt.Month })
                    .GroupBy(g => g.Key.Year)
                    .Select(g => new DashboardResponse.MonthlyRevenueStats
                    {
                        Year = g.Key,
                        MonthlyRevenue = g.ToDictionary(
                            m => m.Key.Month,
                            m => (float)m.Sum(t => t.Amount)
                        )
                    })
                    .ToList();

                foreach (var item in groupedData)
                {
                    for (int month = 1; month <= 12; month++)
                    {
                        if (!item.MonthlyRevenue.ContainsKey(month))
                        {
                            item.MonthlyRevenue[month] = 0f;
                        }
                    }

                    item.MonthlyRevenue = item.MonthlyRevenue
                        .OrderBy(kv => kv.Key)
                        .ToDictionary(kv => kv.Key, kv => kv.Value);
                }

                _logger.LogInformation("Retrieved monthly revenue stats");
                return groupedData.OrderBy(x => x.Year).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting monthly revenue stats");
                throw;
            }
        }

        public async Task<List<DashboardResponse.TotalMonthlyStats>> GetTotalMonthlyStats()
        {
            var appointments = await _appointmentRepository.GetAll().Where(a => !a.IsDeleted && a.Status == "Success").ToListAsync();
            var orders = await _orderRepository.GetAll().Where(o => o.IsSuccessful && !o.IsDeleted).ToListAsync();
            var accountSurveys = await _accountSurveyRepository.GetAll().Where(s => !s.IsDeleted).ToListAsync();

            var allYears = appointments
        .Select(a => a.AppointmentDate.Year)
        .Union(orders.Select(o => o.CreateAt.Year))
        .Union(accountSurveys.Select(s => s.CreateAt.Year))
        .Distinct();

            var result = new List<TotalMonthlyStats>();

            foreach (var year in allYears)
            {
                var data = new TotalMonthlyStats
                {
                    Year = year,
                    AppointmentMonthlyCounts = appointments
                        .Where(a => a.AppointmentDate.Year == year)
                        .GroupBy(a => a.AppointmentDate.Month)
                        .ToDictionary(g => g.Key, g => g.Count()),

                    OrderMonthlyCounts = orders
                        .Where(o => o.CreateAt.Year == year)
                        .GroupBy(o => o.CreateAt.Month)
                        .ToDictionary(g => g.Key, g => g.Count()),

                    SurveyMonthlyCounts = accountSurveys
                        .Where(s => s.CreateAt.Year == year)
                        .GroupBy(s => s.CreateAt.Month)
                        .ToDictionary(g => g.Key, g => g.Count())
                };

                result.Add(data);
            }

            return result;
        }

        public async Task<int> GetTotalAccountsAsync()
        {
            return await _accountRepository.GetAll()
                .Where(a => !a.IsDeleted &&
                           (a.Role.Name == "Student" || a.Role.Name == "Parent"))
                .CountAsync();
        }

        public async Task<int> GetTodayOrderCountAsync()
        {
            var today = DateTime.Today;
            return await _orderRepository.GetAll()
                .Where(o => !o.IsDeleted && o.CreateAt.Date == today)
                .CountAsync();
        }

    }
} 