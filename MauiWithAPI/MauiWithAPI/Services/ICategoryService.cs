using MauiWithAPI.Models;

namespace MauiWithAPI.Services
{
    public interface ICategoryService
    {
        Task<PagedApiResponse<IEnumerable<Category>>> GetCategoriesAsync(int page, int pageSize, string keyword);
        Task<Category> SaveCategoryAsync(Category category, bool isNewCategory = false);
        Task<string> DeleteCategoryAsync(int Id);

    }
}
