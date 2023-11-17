using MauiWithAPI.API.Services.Products.Dto;

namespace MauiWithAPI.API.Services.Products
{
    public interface IProductService
    {
        Task<ApiResponse<PagedApiResponse<IEnumerable<ProductDto>>>> GetProducts(int page, int pageSize, string keyword);
        Task<ApiResponse<ProductDto>> GetProductById(int id);
        Task<ApiResponse<ProductDto>> AddProduct(CreateProductDto productdto);
        Task<ApiResponse<ProductDto>> UpdateProduct(UpdateProductDto productdto);
        Task<ApiResponse<ProductDto>> DeleteProduct(int id);
    }
}
