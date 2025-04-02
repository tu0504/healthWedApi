using HEALTH_SUPPORT.Services.DTOs.Dashboard;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Services.IServices
{
    public interface IDashboardService
    {
        Task<List<MonthlySubscriptionStats>> GetMonthlySubscriptionStats();
        Task<List<MonthlyRevenueStats>> GetMonthlyRevenueStats();
    }
} 