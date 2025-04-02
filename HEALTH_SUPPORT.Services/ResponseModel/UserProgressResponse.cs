using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Services.ResponseModel
{
    public static class UserProgressResponse
    {
        public class GetUserProgressModel
        {
            public Guid Id { get; set; }
            public int Section { get; set; }
            public string Description { get; set; }
            public int Date { get; set; }
            public string SubscriptionName { get; set; }
            public string AccountName { get; set; }
            public bool IsCompleted { get; set; }
            public DateTimeOffset CreateAt { get; set; }
            public DateTimeOffset? ModifiedAt { get; set; }
        }
    }
}
