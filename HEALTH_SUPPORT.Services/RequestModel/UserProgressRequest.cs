using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Services.RequestModel
{
    public static class UserProgressRequest
    {
        public class CreateUserProgressModel
        {
            public int? Section { get; set; }
            public string? Description { get; set; }
            public int? Date { get; set; }

            [Required(ErrorMessage = "SubscriptionId is required.")]
            public Guid SubscriptionId { get; set; }

            [Required(ErrorMessage = "AccountId is required.")]
            public Guid AccountId { get; set; }
        }
        public class UpdateUserProgressModel
        {
            public int? Section { get; set; }
            public string? Description { get; set; }
            public int? Date { get; set; }
            public DateTimeOffset StartDate { get; set; }
            public Guid SubscriptionId { get; set; }
            public Guid AccountId { get; set; }
            public bool IsCompleted { get; set; }
            public bool IsDeleted { get; set; }
        }
    }
}
