using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Services.RequestModel
{
    public static class SubscriptionRequest
    {
        public class CreateSubscriptionModel
        {
            [Required(ErrorMessage = "Thiếu tên chương trình!")]
            public string SubscriptionName { get; set; }

            [Required(ErrorMessage = "Thiếu miêu tả chương trình!")]
            public string Description { get; set; }

            [Required(ErrorMessage = "Thiếu mệnh giá chương trình!")]
            [Range(0.01, float.MaxValue, ErrorMessage = "Mệnh giá chương trình phải lớn hơn 0!")]
            public float Price { get; set; }

            [Required(ErrorMessage = "Thiếu thời hạn chương trình!")]
            [Range(1, int.MaxValue, ErrorMessage = "Thời hạn cần phải nhiều hơn 1 ngày")]
            public int Duration { get; set; }

            [Required(ErrorMessage = "Thiếu loại chương trình!")]
            public string CategoryName { get; set; }

            [Required(ErrorMessage = "Thiếu tên chuyên gia!")]
            public string PsychologistName { get; set; }

            [Required(ErrorMessage = "Thiếu mục đích chương trình!")]
            public string Purpose { get; set; }

            [Required(ErrorMessage = "Thiếu các chỉ số đánh giá của chương trình!")]
            public string Criteria { get; set; }

            [Required(ErrorMessage = "Thiếu nhóm đối tượng của chương trình!")]
            public string FocusGroup { get; set; }

            [Required(ErrorMessage = "Thiếu công cụ đánh giá cho chương trình!")]
            public string AssessmentTool { get; set; }
        }
        public class UpdateSubscriptionModel
        {
            [Required]
            public string Description { get; set; }

            [Required]
            [Range(0.01, float.MaxValue, ErrorMessage = "Mệnh giá chương trình phải lớn hơn 0!")]
            public float Price { get; set; }

            [Required]
            [Range(1, int.MaxValue, ErrorMessage = "Thời hạn cần phải nhiều hơn 1 ngày")]
            public int Duration { get; set; }

            [Required(ErrorMessage = "Vui lòng chọn một chuyên gia tâm lý.")]
            public Guid PsychologistId { get; set; }

            public string? Purpose { get; set; }
            public string? Criteria { get; set; }
            public string? FocusGroup { get; set; }
            public string? AssessmentTool { get; set; }
        }
        
    }
}
