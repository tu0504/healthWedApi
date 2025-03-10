using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Services.ResponseModel
{
    public static class SubscriptionProgressResponse
    {
        public record GetSubscriptionProgressModel(
            Guid Id,
            string Username,
            string SubscriptionName,
            string Description,
            float Price,
            DateTimeOffset StartDate,
            DateTimeOffset? EndDate
        );
    }
}
