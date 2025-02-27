using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Services.RequestModel
{
    public static class AccountRequest
    {
        public class CreateAccountModel
        {
            [Required(ErrorMessage = "Missing Username")]
            public string UserName { get; set; }

            public string Fullname { get; set; }

            [Required(ErrorMessage = "Missing Email")]
            [EmailAddress(ErrorMessage = "Invalid Email Format")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Missing Phone")]
            [Phone(ErrorMessage = "Invalid Phone Number")]
            public string Phone { get; set; }

            public string Address { get; set; }

            [Required(ErrorMessage = "Missing Password")]
            public string PasswordHash { get; set; }

            [Required(ErrorMessage = "Missing Role Name")]
            public string RoleName { get; set; }

        }

        public class UpdateAccountModel
        {
            [Required]
            public Guid AccountId { get; set; }

            public string? Username { get; set; }
            public string? Fullname { get; set; }
            public string? Email { get; set; }
            public string? Phone { get; set; }
            public string? Address { get; set; }
            public string? PasswordHash { get; set; }
        }

        public class LoginRequestModel
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
    }
}
