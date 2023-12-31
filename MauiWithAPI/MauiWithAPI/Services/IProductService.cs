﻿using MauiWithAPI.Models;

namespace MauiWithAPI.Services
{
    public interface IProductService
    {
        Task<PagedApiResponse<IEnumerable<Product>>> GetProductsAsync(int page, int pageSize, string keyword);
        //Task<Product> AddProductAsync(Product product);
        //Task<Product> UpdateProductAsync(Product product);
        Task<Product> SaveProductAsync(Product product, bool isNewProduct = false);
        Task<bool> DeleteProductAsync(int Id);
    }
}
