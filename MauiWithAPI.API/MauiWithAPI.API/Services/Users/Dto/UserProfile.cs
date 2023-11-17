using AutoMapper;
using MauiWithAPI.API.Services.Authentication.Dto;
using Microsoft.AspNetCore.Identity;

namespace MauiWithAPI.API.Services.Users.Dto
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<IdentityUser<int>, UserDto>().ReverseMap();
        }
    }
}
