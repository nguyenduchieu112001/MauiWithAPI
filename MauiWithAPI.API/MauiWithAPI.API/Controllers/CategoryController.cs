using MauiWithAPI.API.Services.Categories;
using MauiWithAPI.API.Services.Categories.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MauiWithAPI.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll([FromQuery] int page, [FromQuery] int pageSize, [FromQuery] string? keyword = null)
        {
            var categories = await _categoryService.GetCategories(page, pageSize, keyword);
            return StatusCode(categories.StatusCode, categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _categoryService.GetCategoryById(id);
            return StatusCode(category.StatusCode, category);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDto createCategoryDto)
        {
            var categoryDto = await _categoryService.AddCategory(createCategoryDto);
            return StatusCode(categoryDto.StatusCode, categoryDto);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryDto categoryDto)
        {
            var category = await _categoryService.UpdateCategory(categoryDto);
            return StatusCode(category.StatusCode, category);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _categoryService.DeleteCategory(id);
            return StatusCode(category.StatusCode, category);
        }
    }
}
