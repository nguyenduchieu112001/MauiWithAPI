using MauiWithAPI.Models;
using System.Net;

namespace MauiWithAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IAuthenService _authenService;
        public UserService(IAuthenService authenService)
        {
            _authenService = authenService;
        }
        public async Task<PagedApiResponse<List<User>>> GetUsersAsync(int page, int pageSize, string keyword)
        {
            var client = await _authenService.GetAuthenticatedHttpClientAsync(AppConstants.HttpUser);
            var uriBuilder = new UriBuilder(client.BaseAddress)
            {
                Query = $"page={page}&pageSize={pageSize}&keyword={WebUtility.UrlEncode(keyword)}"
            };
            var response = await client.GetAsync(uriBuilder.Uri);
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await _authenService.DeserializeApiResponseAsync<PagedApiResponse<List<User>>>(response);
                if (apiResponse.Status)
                {
                    return apiResponse.Data;
                }
            }
            return null;
        }
    }
}
