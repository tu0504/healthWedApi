using System;

namespace HEALTH_SUPPORT.Services.DTOs.Dashboard
{
    public class MonthlyRevenueStats
    {
        public int Month { get; set; }
        public string MonthName { get; set; }
        public float TotalRevenue { get; set; }
    }
} 