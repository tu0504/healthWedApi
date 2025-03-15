using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Services.RequestModel
{
    public static class CategoryRequest
    {
        public class CreateCategoryModel
        {
            [Required(ErrorMessage = "Thiếu tên chuyên mục!")]
            public string CategoryName { get; set; }

            [Required(ErrorMessage = "Thiếu giải thích chuyên mục!")]
            public string Description { get; set; }
        }

        public class UpdateCategoryModel
        {
            [Required]
            public string CategoryName { get; set; }

            [Required]
            public string Description { get; set; }
        }
    }
}
