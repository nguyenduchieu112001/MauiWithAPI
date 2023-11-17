using MauiWithAPI.API.Services.Categories.Dto;

namespace MauiWithAPI.API.Services.Products.Dto
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int Price { get; set; }
        public string ProductImage { get; set; }
        public CategoryDto Category { get; set; }
    }
}
