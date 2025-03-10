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
        public class CreateSubscriptionProgressModel
        {
            [Required(ErrorMessage = "OrderID is required.")]
            public Guid OrderId { get; set; }
        }
    }
}
