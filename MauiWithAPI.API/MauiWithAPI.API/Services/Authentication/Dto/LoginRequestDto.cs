using System.ComponentModel.DataAnnotations;

namespace MauiWithAPI.API.Services.Authentication.Dto
{
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "User Name is required")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
    }
}
