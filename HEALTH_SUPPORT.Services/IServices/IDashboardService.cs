using HEALTH_SUPPORT.Services.ResponseModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Services.IServices
{
    public interface IDashboardService
    {
        Task<DashboardResponse.DashboardStats> GetDashboardStats();
        Task<List<DashboardResponse.MonthlySubscriptionStats>> GetMonthlySubscriptionStats();
        Task<List<DashboardResponse.MonthlyRevenueStats>> GetMonthlyRevenueStats();
        Task<List<DashboardResponse.TotalMonthlyStats>> GetTotalMonthlyStats();
        Task<int> GetTotalAccountsAsync();
        Task<int> GetTodayOrderCountAsync();
    }
} 