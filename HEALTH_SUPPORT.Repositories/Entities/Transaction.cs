using HEALTH_SUPPORT.Repositories.Abstraction;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Repositories.Entities
{
    public class Transaction : Entity<Guid>, IAuditable
    {
        [Required]
        public Guid OrderId { get; set; }
        [ForeignKey("OrderId")]
        public Order Order { get; set; }

        [Required]
        public float Amount { get; set; }

        [Required]
        public string PaymentMethod { get; set; } = "VNPay"; // Default to VNPay for now

        [Required]
        public string PaymentStatus { get; set; } = "pending";

        public string? VnpayTransactionId { get; set; }
        
        public DateTimeOffset PaymentTime { get; set; }
        public DateTimeOffset CreateAt { get; set; }
        public DateTimeOffset? ModifiedAt { get; set; }

        // Additional VNPay specific fields
        public string? VnpayOrderInfo { get; set; }
        public string? VnpayResponseCode { get; set; }
        public string? RedirectUrl { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
