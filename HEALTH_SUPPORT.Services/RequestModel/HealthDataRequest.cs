using HEALTH_SUPPORT.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Services.RequestModel
{
    public class HealthDataRequest
    {
        public class AddHealthDataRequest
        {
            [Required(ErrorMessage = ("Chuẩn đoán không thể bỏ trống"))]
            public string Diagnosis { get; set; }
            public string Purpose { get; set; }
            public string Summary { get; set; }
            [Required(ErrorMessage = ("Tài khoản không thể bỏ trống"))]
            public Guid AccountId { get; set; }
            [Required(ErrorMessage = ("Bác sĩ tâm lý không thể bỏ trống"))]
            public Guid PsychologistId { get; set; }
        }

        public class UpdateHealthDataRequest
        {
            public string Diagnosis { get; set; }
            public string Purpose { get; set; }
            public string Summary { get; set; }
        }
    }
}
