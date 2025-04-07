using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Services.ResponseModel
{
    public class DashboardResponse
    {
        public class AppointmentMonthlyStats
        {
            public int Year { get; set; }
            public Dictionary<int, int> MonthlyCounts { get; set; } = new();
        }

        public class MonthlyRevenueStats
        {
            public int Month { get; set; }
            public string MonthName { get; set; }
            public float TotalRevenue { get; set; }
        }

        public class MonthlySubscriptionStats
        {
            public int Month { get; set; }
            public string MonthName { get; set; }
            public int TotalSubscriptions { get; set; }
        }
    }
}
