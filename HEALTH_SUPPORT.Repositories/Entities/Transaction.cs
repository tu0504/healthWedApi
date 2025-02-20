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
    public class Transaction : Entity<Guid>
    {
        
        [Required]
        public string FullName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public DateTimeOffset TransactionDate { get; set; }

        [Required]
        [MaxLength(100)]
        public string TransactionMethod { get; set; }

        [Required]
        [MaxLength(50)]
        public string TransactionStatus { get; set; }

        [Required]
        public Guid AccountId { get; set; }

        [ForeignKey("AccountId")]
        public Account Account { get; set; }
        public ICollection<Order> Orders { get; set; } = new HashSet<Order>();
    }
}
