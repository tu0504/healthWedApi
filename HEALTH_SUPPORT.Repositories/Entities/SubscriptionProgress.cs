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
    public class SubscriptionProgress : Entity<Guid>, IAuditable
    {
        public int? Section { get; set; }
        public string? Description { get; set; }
        public int? Date { get; set; }
        public DateTimeOffset? StartDate { get; set; }

        [Required]
        public Guid SubscriptionId { get; set; }

        [ForeignKey("SubscriptionId")]
        public SubscriptionData SubscriptionDatas { get; set; }
        public bool IsCompleted { get; set; }
        public DateTimeOffset CreateAt { get; set; }
        public DateTimeOffset? ModifiedAt { get; set; }
    }
}
