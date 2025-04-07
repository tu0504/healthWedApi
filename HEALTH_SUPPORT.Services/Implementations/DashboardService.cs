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

        public async Task<DashboardResponse.MonthlyRevenueStats> GetMonthlyRevenueStats()
        {
            try
            {
                var currentYear = DateTime.Now.Year;
                var transactions = await _transactionRepository.GetAll()
                    .Where(t => t.PaymentStatus == "success" && !t.IsDeleted && t.CreateAt.Year == currentYear)
                    .ToListAsync();

                var monthlyRevenue = new Dictionary<int, float>();

                // Initialize all months with zero revenue
                for (int month = 1; month <= 12; month++)
                {
                    monthlyRevenue[month] = 0f;
                }

                // Fill in actual revenue for months that have transactions
                var revenueByMonth = transactions
                    .GroupBy(t => t.CreateAt.Month)
                    .Select(g => new { Month = g.Key, Total = (float)g.Sum(t => t.Amount) });

                foreach (var revenue in revenueByMonth)
                {
                    monthlyRevenue[revenue.Month] = revenue.Total;
                }

                _logger.LogInformation("Retrieved monthly revenue stats for year {Year}", currentYear);
                return new DashboardResponse.MonthlyRevenueStats
                {
                    Year = currentYear,
                    MonthlyRevenue = monthlyRevenue
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting monthly revenue stats");
                throw;
            }
        }
    }
} 