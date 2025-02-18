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
    public class ProgramProgress : Entity<Guid>
    {


        [Range(0, 100)]
        public double CompletePercent { get; set; } = 0;

        public DateTimeOffset StartDate { get; set; } = DateTimeOffset.UtcNow;

        public DateTimeOffset? EndDate { get; set; }

        [Required]
        public Guid AccountId { get; set; }

        [Required]
        public Guid ProgramDataId { get; set; }

        [ForeignKey("AccountId")]
        public Account Account { get; set; }

        [ForeignKey("ProgramId")]
        public ProgramData ProgramData { get; set; }
    }
}