using AutoMapper;
using MauiWithAPI.API.Models;

namespace MauiWithAPI.API.Services.Categories.Dto
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CategoryDto, Category>().ReverseMap();
        }
    }
}
