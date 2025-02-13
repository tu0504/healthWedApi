using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Models
{
    public class Account
    {
        [Key]
        public int AccountId { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        public int RoleId { get; set; }
        public bool IsDeleted { get; set; }
        [ForeignKey("RoleId")]
        public Role Role { get; set; }
    }
}
