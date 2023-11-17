using MauiWithAPI.API.Services.Categories.Dto;

namespace MauiWithAPI.API.Services.Categories
{
    public interface ICategoryService
    {
        Task<ApiResponse<PagedApiResponse<IEnumerable<CategoryDto>>>> GetCategories(int page, int pageSize, string keyword);
        Task<ApiResponse<CategoryDto>> GetCategoryById(int id);
        Task<ApiResponse<CategoryDto>> AddCategory(CreateCategoryDto category);
        Task<ApiResponse<CategoryDto>> UpdateCategory(UpdateCategoryDto category);
        Task<ApiResponse<CategoryDto>> DeleteCategory(int id);
    }
}
