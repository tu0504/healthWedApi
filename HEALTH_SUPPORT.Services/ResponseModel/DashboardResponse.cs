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
    }
}
