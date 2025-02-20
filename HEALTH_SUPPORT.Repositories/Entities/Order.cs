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
    public class Order : Entity<Guid>
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice => UnitPrice * Quantity;

        [Required]
        [MaxLength(50)]
        public string OrderStatus { get; set; }

        [Required]
        public Guid SubscriptionId { get; set; }

        [Required]
        public Guid TransactionId { get; set; }

        [ForeignKey("SubscriptionId")]
        public SubscriptionData Subscription { get; set; } = new SubscriptionData();

        [ForeignKey("TransactionId")]
        public Transaction Transaction { get; set; } = new Transaction();
    }
}
