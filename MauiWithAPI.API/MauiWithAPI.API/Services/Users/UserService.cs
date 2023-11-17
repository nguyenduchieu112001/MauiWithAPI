using AutoMapper;
using MauiWithAPI.API.Models;
using MauiWithAPI.API.Services.Authentication.Dto;
using MauiWithAPI.API.Services.Users.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MauiWithAPI.API.Services.Users
{
    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser<int>> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly IMapper _mapper;
        public UserService(
            UserManager<IdentityUser<int>> userManager,
            IMapper mapper,
            RoleManager<IdentityRole<int>> roleManager)
        {
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
        }
        public async Task<ApiResponse<PagedApiResponse<List<UserDto>>>> GetUsersAsync(int page, int pageSize, string keyword)
        {
            {
                var query = _userManager.Users.AsQueryable();
                int totalCount = query.Count();

                if (!string.IsNullOrEmpty(keyword))
                {
                    query = query.Where(p => p.UserName.Contains(keyword) ||
                                        p.Email.Contains(keyword));
                }

                if (page == 0)
                    page = 1;
                if (pageSize == 0)
                    pageSize = totalCount;
                int skip = (page - 1) * pageSize;
                int take = pageSize;

                var users = await query.OrderBy(u => u.UserName).Skip(skip).Take(pageSize).ToListAsync();

                var userDtos = new List<UserDto>();
                foreach (var user in users)
                {
                    var role = await _roleManager.FindByNameAsync(UserRoles.Admin);
                    var roleDto = new RoleDto
                    {
                        Id = role.Id,
                        RoleName = role.Name,
                    };
                    var userDto = _mapper.Map<UserDto>(user);
                    userDto.Role = roleDto;
                    userDtos.Add(userDto);
                }

                int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

                var response = new PagedApiResponse<List<UserDto>>
                {
                    Data = userDtos,
                    TotalItems = totalCount,
                    Page = page,
                    PageSize = pageSize,
                    TotalPages = totalPages,
                };
                return ApiResponse<PagedApiResponse<List<UserDto>>>.Success(response);
            }
        }
    }
}
