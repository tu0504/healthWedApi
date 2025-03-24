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
            public DateTimeOffset CreateAt { get; set; }
        }

        public class UpdateOrderModel
        {
            public Guid SubscriptionDataId { get; set; }
            public int Quantity { get; set; }
            public bool IsJoined { get; set; }
            public bool IsSuccessful { get; set; }
            public bool IsDeleted { get; set; }
            public DateTimeOffset? ModifiedAt { get; set; }
        }

    }
}
