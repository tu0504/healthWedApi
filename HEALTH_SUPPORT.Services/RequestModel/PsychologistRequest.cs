using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Services.RequestModel
{
    public class PsychologistRequest
    {
        public class CreatePsychologistModel
        {
            [Required(ErrorMessage = "Thiếu họ và tên!")]
            public string Name { get; set; }

            [Required(ErrorMessage = "Thiếu Email!")]
            [EmailAddress(ErrorMessage = "Định dạng Email không hợp lệ!")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Thiếu số điện thoại!")]
            [Phone(ErrorMessage = "Định dạng số điện thoại không hợp lệ!")]
            public string PhoneNumber { get; set; }

            [Required(ErrorMessage = "Thiếu chuyên môn!")]
            public string Specialization { get; set; }
        }

        public class UpdatePsychologistModel
        {
            public string? Name { get; set; }
            public string? Email { get; set; }
            public string? PhoneNumber { get; set; }
            public string? Specialization { get; set; }
        }
    }
}
