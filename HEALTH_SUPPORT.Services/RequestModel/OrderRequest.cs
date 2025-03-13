using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Services.RequestModel
{
    public static class OrderRequest
    {
        public class CreateOrderModel
        {
            [Required(ErrorMessage = "SubscriptionId is required.")]
            public Guid SubscriptionId { get; set; }

            [Required(ErrorMessage = "AccountId is required.")]
            public Guid AccountId { get; set; }

            [Required]
            [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
            public int Quantity { get; set; }
        }
    }
}
