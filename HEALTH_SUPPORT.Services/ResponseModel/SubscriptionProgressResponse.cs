using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Services.ResponseModel
{
    public static class SubscriptionProgressResponse
    {
        public class GetProgressModel
        {
            public Guid Id { get; set; }
            public int Section { get; set; }
            public string Description { get; set; }
            public int Date { get; set; }
            public string SubscriptionName { get; set; }
            public DateTimeOffset CreateAt { get; set; }
            public DateTimeOffset? ModifiedAt { get; set; }
        }
    }
}
