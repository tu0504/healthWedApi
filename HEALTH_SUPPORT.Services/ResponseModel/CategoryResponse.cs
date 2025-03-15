using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Services.ResponseModel
{
    public static class CategoryResponse
    {
        public record GetCategoryModel(
            Guid Id,
            string CategoryName,
            string Description
            );
    }
}
