using MauiWithAPI.Models;

namespace MauiWithAPI.Services
{
    public interface ICategoryService
    {
        Task<PagedApiResponse<IEnumerable<Category>>> GetCategoriesAsync(int page, int pageSize, string keyword);
        Task<Category> AddCategoryAsync(Category category);
        Task<Category> UpdateCategoryAsync(Category category);
        Task<string> DeleteCategoryAsync(int Id);

    }
}
