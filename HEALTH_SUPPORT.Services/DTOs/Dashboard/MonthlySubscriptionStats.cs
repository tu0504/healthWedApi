using System;

namespace HEALTH_SUPPORT.Services.DTOs.Dashboard
{
    public class MonthlySubscriptionStats
    {
        public int Month { get; set; }
        public string MonthName { get; set; }
        public int TotalSubscriptions { get; set; }
    }
} 