using HEALTH_SUPPORT.Repositories.Abstraction;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Repositories.Entities
{
    public class Account : Entity<Guid>, IAuditable
    {
        [Required]
        public string FullName { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        [Required]
        public string Password { get; set; }
        public DateTimeOffset? ModifiedAt { get; set; }
        public DateTimeOffset CreateAt { get; set; }
        public ICollection<ProgramProgress> ProgramProgresses { get; set; }
        public ICollection<ProgramRegistration> ProgramRegistrations { get; set; }
        public ICollection<AccountSurvey> AccountSurveys { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
        public Guid RoleId { get; set; }
        [ForeignKey("RoleId")]
        public Role Role { get; set; }

    }
}
