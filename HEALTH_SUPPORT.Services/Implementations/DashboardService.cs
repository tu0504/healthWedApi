using HEALTH_SUPPORT.Repositories;
using HEALTH_SUPPORT.Repositories.Entities;
using HEALTH_SUPPORT.Repositories.Repository;
using HEALTH_SUPPORT.Services.DTOs.Dashboard;
using HEALTH_SUPPORT.Services.IServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Services.Implementations
{
    public class DashboardService : IDashboardService
    {
        private readonly IBaseRepository<Order, Guid> _orderRepository;
        private readonly IBaseRepository<Transaction, Guid> _transactionRepository;
        private readonly ILogger<DashboardService> _logger;

        public DashboardService(
            IBaseRepository<Order, Guid> orderRepository,
            IBaseRepository<Transaction, Guid> transactionRepository,
            ILogger<DashboardService> logger)
        {
            _orderRepository = orderRepository;
            _transactionRepository = transactionRepository;
            _logger = logger;
        }

        public async Task<List<MonthlySubscriptionStats>> GetMonthlySubscriptionStats()
        {
            try
            {
                var currentYear = DateTime.Now.Year;
                var stats = await _orderRepository.GetAll()
                    .Where(o => o.IsSuccessful && !o.IsDeleted && o.CreateAt.Year == currentYear)
                    .GroupBy(o => new { Month = o.CreateAt.Month })
                    .Select(g => new MonthlySubscriptionStats
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

        public async Task<List<MonthlyRevenueStats>> GetMonthlyRevenueStats()
        {
            try
            {
                var currentYear = DateTime.Now.Year;
                var stats = await _transactionRepository.GetAll()
                    .Where(t => t.PaymentStatus == "success" && !t.IsDeleted && t.CreateAt.Year == currentYear)
                    .GroupBy(t => new { Month = t.CreateAt.Month })
                    .Select(g => new MonthlyRevenueStats
                    {
                        Month = g.Key.Month,
                        MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key.Month),
                        TotalRevenue = g.Sum(t => t.Amount)
                    })
                    .OrderBy(s => s.Month)
                    .ToListAsync();

                _logger.LogInformation("Retrieved monthly revenue stats for year {Year}", currentYear);
                return stats;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting monthly revenue stats");
                throw;
            }
        }
    }
} 