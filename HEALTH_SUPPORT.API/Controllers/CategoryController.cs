using HEALTH_SUPPORT.Services.IServices;
using HEALTH_SUPPORT.Services.RequestModel;
using Microsoft.AspNetCore.Mvc;

namespace HEALTH_SUPPORT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryRequest.CreateCategoryModel model)
        {
            await _categoryService.AddCategory(model);
            return Ok(new { message = "Category created successfully!" });
        }

        [HttpGet(Name = "GetCategories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetCategories()
        {
            var result = await _categoryService.GetCategory();
            return Ok(result);
        }

        [HttpGet("{categoryId}", Name = "GetCategoryById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetCategoryById(Guid categoryId)
        {
            var result = await _categoryService.GetCategoryById(categoryId);
            if (result == null)
            {
                return NotFound(new { message = "Category not found" });
            }
            return Ok(result);
        }

        [HttpPut("{categoryId}", Name = "UpdateCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateCategory(Guid categoryId, [FromBody] CategoryRequest.UpdateCategoryModel model)
        {
            if (model == null)
            {
                return BadRequest(new { message = "Invalid update data" });
            }

            var existingCategory = await _categoryService.GetCategoryByIdDelete(categoryId);
            if (existingCategory == null)
            {
                return NotFound(new { message = "Category not found" });
            }

            await _categoryService.UpdateCategory(categoryId, model);
            return Ok(new { message = "Category updated successfully" });
        }

        [HttpDelete("{categoryId}", Name = "DeleteCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteCategory(Guid categoryId)
        {
            var existingCategory = await _categoryService.GetCategoryById(categoryId);
            if (existingCategory == null)
            {
                return NotFound(new { message = "Category not found" });
            }
            await _categoryService.RemoveCategory(categoryId);
            return Ok(new { message = "Category deleted successfully" });
        }
    }
}
