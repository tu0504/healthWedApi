using HEALTH_SUPPORT.Services.ResponseModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Services.IServices
{
    public interface IDashboardService
    {
        Task<List<DashboardResponse.MonthlySubscriptionStats>> GetMonthlySubscriptionStats();
        Task<List<DashboardResponse.MonthlyRevenueStats>> GetMonthlyRevenueStats();
    }
} 