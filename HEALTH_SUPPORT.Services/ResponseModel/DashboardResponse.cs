using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Services.ResponseModel
{
    public class DashboardResponse
    {
        public class TotalMonthlyStats
        {
            public int Year { get; set; }
            public Dictionary<int, int> AppointmentMonthlyCounts { get; set; } = new();
            public Dictionary<int, int> OrderMonthlyCounts { get; set; } = new();
            public Dictionary<int, int> SurveyMonthlyCounts { get; set; } = new();
        }

        public class MonthlyRevenueStats
        {
            public int Year { get; set; }
            public Dictionary<int, float> MonthlyRevenue { get; set; } = new();
        }

        public class MonthlySubscriptionStats
        {
            public int Month { get; set; }
            public string MonthName { get; set; }
            public int TotalSubscriptions { get; set; }
        }

        public class DashboardStats
        {
            public int Year { get; set; }
            public Dictionary<int, float> MonthlyRevenue { get; set; } = new();
            public Dictionary<int, Dictionary<string, int>> MonthlySubscriptions { get; set; } = new();
        }
    }
}
