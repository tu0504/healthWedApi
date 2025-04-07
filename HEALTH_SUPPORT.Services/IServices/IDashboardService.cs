using HEALTH_SUPPORT.Services.ResponseModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Services.IServices
{
    public interface IDashboardService
    {
        Task<DashboardResponse.DashboardStats> GetDashboardStats();
        Task<List<DashboardResponse.MonthlySubscriptionStats>> GetMonthlySubscriptionStats();
        Task<DashboardResponse.MonthlyRevenueStats> GetMonthlyRevenueStats();
    }
} 