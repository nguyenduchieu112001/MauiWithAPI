using AutoMapper;
using MauiWithAPI.API.DataContext;
using MauiWithAPI.API.Models;
using MauiWithAPI.API.Services.Categories.Dto;
using Microsoft.EntityFrameworkCore;

namespace MauiWithAPI.API.Services.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        public CategoryService(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ApiResponse<PagedApiResponse<IEnumerable<CategoryDto>>>> GetCategories(int page, int pageSize, string keyword)
        {
            var query = _dbContext.Categories.AsQueryable();
            int totalCount = await _dbContext.Categories.CountAsync();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(p => p.CategoryName.Contains(keyword));
            }

            if (page == 0)
                page = 1;
            if (pageSize == 0)
                pageSize = totalCount;
            int skip = (page - 1) * pageSize;
            int take = pageSize;

            var categories = await query.OrderBy(c => c.CategoryName)
                                            .Skip(skip).Take(take).ToListAsync();


            int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var response = new PagedApiResponse<IEnumerable<CategoryDto>>
            {
                Data = _mapper.Map<IEnumerable<CategoryDto>>(categories),
                Page = page,
                PageSize = pageSize,
                TotalItems = totalCount,
                TotalPages = totalPages
            };
            return ApiResponse<PagedApiResponse<IEnumerable<CategoryDto>>>.Success(response);
        }

        public async Task<ApiResponse<CategoryDto>> GetCategoryById(int id)
        {
            var category = await _dbContext.Categories.FindAsync(id);
            if (category is null)
            {
                return ApiResponse<CategoryDto>.Failure("Not found", 404);
            }
            var categoryDto = _mapper.Map<CategoryDto>(category);
            return ApiResponse<CategoryDto>.Success(categoryDto);
        }

        public async Task<ApiResponse<CategoryDto>> AddCategory(CreateCategoryDto category)
        {
            var newCategory = new Category()
            {
                CategoryName = category.CategoryName,
            };
            var result = await _dbContext.AddAsync(newCategory);
            await _dbContext.SaveChangesAsync();
            var categoryDto = _mapper.Map<CategoryDto>(result.Entity);
            return ApiResponse<CategoryDto>.Success(categoryDto);
        }

        public async Task<ApiResponse<CategoryDto>> UpdateCategory(UpdateCategoryDto category)
        {
            var updateCategory = await _dbContext.Categories.FindAsync(category.Id);
            if (updateCategory is null)
            {
                return ApiResponse<CategoryDto>.Failure("Not found", 404);
            }
            updateCategory.CategoryName = category.CategoryName;
            await _dbContext.SaveChangesAsync();
            var categoryDto = _mapper.Map<CategoryDto>(updateCategory);
            return ApiResponse<CategoryDto>.Success(categoryDto);

        }

        public async Task<ApiResponse<CategoryDto>> DeleteCategory(int id)
        {
            var category = await _dbContext.Categories.FindAsync(id);
            if (category is null)
            {
                return ApiResponse<CategoryDto>.Failure("Not found", 404);
            }
            var product = await _dbContext.Products.CountAsync(p => p.CategoryId == id);
            if (product > 0)
            {
                return ApiResponse<CategoryDto>.Failure($"Product is using {category.CategoryName}", 400);
            }
            _dbContext.Categories.Remove(category);
            await _dbContext.SaveChangesAsync();
            return ApiResponse<CategoryDto>.Success(200);
        }
    }
}
