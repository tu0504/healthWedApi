using HEALTH_SUPPORT.Services.RequestModel;
using HEALTH_SUPPORT.Services.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Services.IServices
{
    public interface ICategoryService
    {
        Task<List<CategoryResponse.GetCategoryModel>> GetCategory();
        Task<CategoryResponse.GetCategoryModel?> GetCategoryById(Guid id);
        Task AddCategory(CategoryRequest.CreateCategoryModel model);
        Task UpdateCategory(Guid id, CategoryRequest.UpdateCategoryModel model);
        Task RemoveCategory(Guid id);
    }
}
