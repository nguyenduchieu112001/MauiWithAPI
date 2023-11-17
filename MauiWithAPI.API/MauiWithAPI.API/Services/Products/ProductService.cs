using AutoMapper;
using MauiWithAPI.API.DataContext;
using MauiWithAPI.API.Models;
using MauiWithAPI.API.Services.Files;
using MauiWithAPI.API.Services.Products.Dto;
using Microsoft.EntityFrameworkCore;

namespace MauiWithAPI.API.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;
        public ProductService(
            ApplicationDbContext dbContext,
            IMapper mapper,
            IFileService fileService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _fileService = fileService;
        }
        public async Task<ApiResponse<ProductDto>> AddProduct(CreateProductDto product)
        {
            var category = await _dbContext.Categories.FindAsync(product.CategoryId);
            if (category == null)
            {
                return ApiResponse<ProductDto>.Failure("Not found", 404);
            }
            var newProduct = new Product
            {
                ProductName = product.ProductName!,
                Price = product.Price,
                CategoryId = product.CategoryId,
            };
            if (product.ImageFile != null)
            {
                var fileResult = _fileService.SaveImage(product.ImageFile);
                if (fileResult.Item1 == 1)
                {
                    newProduct.ProductImage = fileResult.Item2;
                }
            }
            var result = await _dbContext.AddAsync(newProduct);
            await _dbContext.SaveChangesAsync();
            var productDto = _mapper.Map<ProductDto>(result.Entity);
            return ApiResponse<ProductDto>.Success(productDto);
        }

        public async Task<ApiResponse<ProductDto>> DeleteProduct(int id)
        {
            var product = await _dbContext.Products.FindAsync(id);
            if (product == null)
            {
                return ApiResponse<ProductDto>.Failure("Not found", 404);
            }
            _fileService.DeleteImage(product.ProductImage);
            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();
            return ApiResponse<ProductDto>.Success(200);
        }

        public async Task<ApiResponse<ProductDto>> GetProductById(int id)
        {
            var product = await _dbContext.Products.FindAsync(id);
            if (product == null)
            {
                return ApiResponse<ProductDto>.Failure("Not found", 404);
            }
            var productDto = _mapper.Map<ProductDto>(product);
            return ApiResponse<ProductDto>.Success(productDto);
        }

        public async Task<ApiResponse<PagedApiResponse<IEnumerable<ProductDto>>>> GetProducts(int page, int pageSize, string keyword)
        {
            var query = _dbContext.Products.Include(p => p.Category).AsQueryable();
            int totalCount = await _dbContext.Products.CountAsync();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(p => p.ProductName.Contains(keyword) ||
                                    p.Category.CategoryName.Contains(keyword));
            }

            if (page == 0)
                page = 1;
            if (pageSize == 0)
                pageSize = totalCount;

            int skip = (page - 1) * pageSize;
            int take = pageSize;

            var products = await query.AsNoTracking().OrderBy(c => c.ProductName).Skip(skip)
                            .Take(take).ToListAsync();

            int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var response = new PagedApiResponse<IEnumerable<ProductDto>>
            {
                Data = _mapper.Map<IEnumerable<ProductDto>>(products),
                Page = page,
                PageSize = pageSize,
                TotalItems = totalCount,
                TotalPages = totalPages
            };

            return ApiResponse<PagedApiResponse<IEnumerable<ProductDto>>>.Success(response);
        }

        public async Task<ApiResponse<ProductDto>> UpdateProduct(UpdateProductDto product)
        {
            var updateProduct = await _dbContext.Products.FindAsync(product.Id);
            if (product == null)
            {
                return ApiResponse<ProductDto>.Failure("Not found", 404);
            }
            updateProduct!.ProductName = product.ProductName!;
            updateProduct.Price = product.Price;
            updateProduct.CategoryId = product.CategoryId;
            if (product.ImageFile != null)
            {
                var isDeleted = _fileService.DeleteImage(updateProduct.ProductImage);
                if (isDeleted)
                {
                    if (product.ImageFile != null)
                    {
                        var fileResult = _fileService.SaveImage(product.ImageFile);
                        if (fileResult.Item1 == 1)
                        {
                            updateProduct.ProductImage = fileResult.Item2;
                        }
                    }
                }
                else
                {
                    return ApiResponse<ProductDto>.Failure("Not found Image", 404);
                }
            }
            await _dbContext.SaveChangesAsync();
            var productDto = _mapper.Map<ProductDto>(updateProduct);
            return ApiResponse<ProductDto>.Success(productDto);
        }
    }
}
