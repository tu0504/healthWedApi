using HEALTH_SUPPORT.Repositories.Entities;
using HEALTH_SUPPORT.Repositories.Repository;
using HEALTH_SUPPORT.Services.IServices;
using HEALTH_SUPPORT.Services.RequestModel;
using HEALTH_SUPPORT.Services.ResponseModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEALTH_SUPPORT.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly IBaseRepository<Category, Guid> _categoryRepository;
        public CategoryService(IBaseRepository<Category, Guid> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task AddCategory(CategoryRequest.CreateCategoryModel model)
        {
            var newCategory = new Category
            {
                Id = Guid.NewGuid(),
                CategoryName = model.CategoryName,
                Description = model.Description,
                IsDeleted = false
            };

            await _categoryRepository.Add(newCategory);
            await _categoryRepository.SaveChangesAsync();
        }

        public async Task<List<CategoryResponse.GetCategoryModel>> GetCategory()
        {
            return await _categoryRepository.GetAll()
                .Where(c => !c.IsDeleted) // Exclude deleted categories
                .Select(c => new CategoryResponse.GetCategoryModel(
                    c.Id,
                    c.CategoryName,
                    c.Description
                ))
                .ToListAsync();
        }

        public async Task<CategoryResponse.GetCategoryModel?> GetCategoryById(Guid id)
        {
            var category = await _categoryRepository.GetById(id);
            if (category == null || category.IsDeleted)
            {
                return null;
            }

            return new CategoryResponse.GetCategoryModel(
                category.Id,
                category.CategoryName,
                category.Description
            );
        }

        public async Task UpdateCategory(Guid id, CategoryRequest.UpdateCategoryModel model)
        {
            var existingCategory = await _categoryRepository.GetById(id);
            if (existingCategory == null || existingCategory.IsDeleted)
            {
                throw new InvalidOperationException("Category not found.");
            }

            existingCategory.CategoryName = string.IsNullOrWhiteSpace(model.CategoryName)
                ? existingCategory.CategoryName
                : model.CategoryName;

            existingCategory.Description = string.IsNullOrWhiteSpace(model.Description)
                ? existingCategory.Description
                : model.Description;

            await _categoryRepository.Update(existingCategory);
            await _categoryRepository.SaveChangesAsync();
        }
        public async Task RemoveCategory(Guid id)
        {
            var category = await _categoryRepository.GetById(id);

            if (category == null || category.IsDeleted)
            {
                throw new InvalidOperationException("Category not found.");
            }

            category.IsDeleted = true;
            await _categoryRepository.Update(category);
            await _categoryRepository.SaveChangesAsync();
        }

    }
}
