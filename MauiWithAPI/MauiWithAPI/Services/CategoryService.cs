﻿using MauiWithAPI.Models;
using System.Net;
using System.Net.Http.Json;

namespace MauiWithAPI.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IAuthenService _authenService;
        public CategoryService(IAuthenService authenService)
        {
            _authenService = authenService;
        }

        public async Task<string> DeleteCategoryAsync(int Id)
        {
            using var client = await _authenService.GetAuthenticatedHttpClientAsync(AppConstants.HttpCategory);
            var response = await client.DeleteAsync($"{client.BaseAddress}/{Id}");
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return AppConstants.Unauthorized;
                }
                var apiResponse = await _authenService.DeserializeApiResponseAsync<Category>(response);
                return apiResponse.Errors.FirstOrDefault()?.ToString();
            }
            return null;
        }

        public async Task<PagedApiResponse<IEnumerable<Category>>> GetCategoriesAsync(int page, int pageSize, string keyword)
        {
            using var client = await _authenService.GetAuthenticatedHttpClientAsync(AppConstants.HttpCategory);
            var uriBuilder = new UriBuilder(client.BaseAddress)
            {
                Query = $"page={page}&pageSize={pageSize}&keyword={WebUtility.UrlEncode(keyword)}"
            };
            var response = await client.GetAsync(uriBuilder.Uri);
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await _authenService.DeserializeApiResponseAsync<PagedApiResponse<IEnumerable<Category>>>(response);
                if (apiResponse.Status)
                {
                    return apiResponse.Data;
                }
            }
            return null;
        }

        public async Task<Category> SaveCategoryAsync(Category category, bool isNewCategory = false)
        {
            using var client = await _authenService.GetAuthenticatedHttpClientAsync(AppConstants.HttpCategory);
            HttpResponseMessage response;
            if (isNewCategory)
            {
                response = await client.PostAsJsonAsync(client.BaseAddress, category);
            }
            else
            {
                response = await client.PutAsJsonAsync(client.BaseAddress, category);
            }
            if (response != null && response.IsSuccessStatusCode)
            {
                var apiResponse = await _authenService.DeserializeApiResponseAsync<Category>(response);
                if (apiResponse.Status)
                {
                    return apiResponse.Data;
                }
            }
            return null;
        }
    }
}
