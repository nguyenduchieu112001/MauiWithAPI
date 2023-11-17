using MauiWithAPI.Models;
using System.Net;

namespace MauiWithAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IAuthenService _authenService;
        public ProductService(IAuthenService authenService)
        {
            _authenService = authenService;
        }
        public async Task<Product> AddProductAsync(Product product)
        {
            var client = await _authenService.GetAuthenticatedHttpClientAsync(AppConstants.HttpProduct);
            var formContent = new MultipartFormDataContent
            {
                { new StringContent(product.ProductName), "ProductName" },
                { new StringContent(product.Price.ToString()), "Price" },
                { new StringContent(product.CategoryId.ToString()), "CategoryId" },
                { new StreamContent(File.OpenRead(product.ProductImage)), "ImageFile",
                    Path.GetFileName(product.ProductImage) }
            };
            var response = await client.PostAsync(client.BaseAddress, formContent);
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await _authenService.DeserializeApiResponseAsync<Product>(response);
                if (apiResponse.Status)
                {
                    return apiResponse.Data;
                }
            }
            return null;
        }

        public async Task<bool> DeleteProductAsync(int Id)
        {
            var client = await _authenService.GetAuthenticatedHttpClientAsync(AppConstants.HttpProduct);
            var response = await client.DeleteAsync($"{client.BaseAddress}/{Id}");
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public async Task<PagedApiResponse<IEnumerable<Product>>> GetProductsAsync(int page, int pageSize, string keyword)
        {
            var client = await _authenService.GetAuthenticatedHttpClientAsync(AppConstants.HttpProduct);
            var uriBuilder = new UriBuilder(client.BaseAddress)
            {
                Query = $"page={page}&pageSize={pageSize}&keyword={WebUtility.UrlEncode(keyword)}"
            };
            var response = await client.GetAsync(uriBuilder.Uri);
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await _authenService.DeserializeApiResponseAsync<PagedApiResponse<IEnumerable<Product>>>(response);
                if (apiResponse.Status)
                {
                    return apiResponse.Data;
                }
            }
            return null;
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            var client = await _authenService.GetAuthenticatedHttpClientAsync(AppConstants.HttpProduct);
            var formContent = new MultipartFormDataContent
            {
                { new StringContent(product.Id.ToString()), "Id" },
                { new StringContent(product.ProductName), "ProductName" },
                { new StringContent(product.Price.ToString()), "Price" },
                { new StringContent(product.CategoryId.ToString()), "CategoryId" },
            };

            if (product.ProductImage != null)
            {
                using (var fileStream = File.OpenRead(product.ProductImage))
                {
                    formContent.Add(new StreamContent(fileStream), "ImageFile", Path.GetFileName(product.ProductImage));
                }
            }
            var response = await client.PutAsync(client.BaseAddress, formContent);
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await _authenService.DeserializeApiResponseAsync<Product>(response);
                if (apiResponse.Status)
                {
                    return apiResponse.Data;
                }
            }
            return null;
        }
    }
}
