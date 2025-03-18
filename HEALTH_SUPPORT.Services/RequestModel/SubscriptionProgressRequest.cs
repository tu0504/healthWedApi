using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Services.RequestModel
{
    public static class SubscriptionProgressRequest
    {
        public class CreateProgressModel
        {
            public string Description { get; set; }
            public int Date { get; set; }

            [Required(ErrorMessage = "SubscriptionId is required.")]
            public Guid SubscriptionId { get; set; }
            public bool IsDelete { get; set; }
        }

        public class UpdateProgressModel
        {
            public string Description { get; set; }
            public int Date { get; set; }
            public Guid SubscriptionId { get; set; }
            public bool IsDelete { get; set; }
        }
    }
}
