﻿namespace MauiWithAPI.API.Services.Products.Dto
{
    public class CreateProductDto
    {
        public string? ProductName { get; set; }
        public int Price { get; set; }
        public int CategoryId { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
