using AutoMapper;
using MauiWithAPI.API.Models;

namespace MauiWithAPI.API.Services.Products.Dto
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductDto, Product>().ReverseMap();
            CreateMap<Product, ProductDto>()
                 .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category));
        }
    }
}
