using Microsoft.AspNetCore.Http;
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

            [Required(ErrorMessage = "Thiếu mô tả!")]
            public string Description { get; set; }

            [Required(ErrorMessage = "Thiếu thành tựu!")]
            public string Achievements { get; set; }

            [Required]
            [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự!")]
            public string PasswordHash { get; set; }
            [Required]
            [Compare("PasswordHash", ErrorMessage = "Mật khẩu nhập lại không khớp!")]
            public string ConfirmPassword { get; set; }
        }

        public class UpdatePsychologistModel
        {
            public string? Name { get; set; }
            public string? Email { get; set; }
            public string? PhoneNumber { get; set; }
            public string? Specialization { get; set; }
            public string? Description { get; set; }
            public string? Achievements { get; set; }
        }

        public class UploadAvatarModel
        {
            public IFormFile File { get; set; }
        }
    }
}
