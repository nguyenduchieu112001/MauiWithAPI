using MauiWithAPI.API.Services.Users.Dto;

namespace MauiWithAPI.API.Services.Authentication.Dto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public RoleDto Role { get; set; }
    }
}
