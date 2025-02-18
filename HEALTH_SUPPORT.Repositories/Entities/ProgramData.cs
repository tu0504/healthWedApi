using HEALTH_SUPPORT.Repositories.Abstraction;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Repositories.Entities
{
    public class ProgramData : Entity<Guid>, IAuditable
    {
        [Required]
        [MaxLength(255)]
        public string ProgramName { get; set; }

        public string Description { get; set; }

        [Required]
        public int Duration { get; set; } // Duration in days

        public DateTimeOffset StartDate { get; set; }

        public DateTimeOffset? EndDate { get; set; }
        public DateTimeOffset CreateAt { get; set; }
        public DateTimeOffset? ModifiedAt { get; set; }
        public ICollection<ProgramProgress> ProgramProgresses { get; set; }

        public ICollection<ProgramRegistration> ProgramRegistrations { get; set; }

     
       
    }
}
