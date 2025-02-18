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
    public class ProgramRegistration : Entity<Guid>
    {


        [Required]
        public DateTimeOffset RegistrationDate { get; set; } = DateTimeOffset.UtcNow;

        [Required]
        [MaxLength(50)]
        public string RegistrationStatus { get; set; } = "Pending"; // Example: Pending, Approved, Rejected

        [Required]
        public Guid AccountId { get; set; }

        [Required]
        public Guid ProgramDataId { get; set; }

        [ForeignKey("AccountId")]
        public Account Account { get; set; }

        [ForeignKey("ProgramDataId")]
        public ProgramData ProgramData { get; set; }
    }
}
