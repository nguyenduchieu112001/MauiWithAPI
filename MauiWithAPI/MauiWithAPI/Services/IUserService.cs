using MauiWithAPI.Models;

namespace MauiWithAPI.Services
{
    public interface IUserService
    {
        Task<PagedApiResponse<List<User>>> GetUsersAsync(int page, int pageSize, string keyword);
    }
}
