using MauiWithAPI.API.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MauiWithAPI.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        public async Task<ActionResult> GetUsers([FromQuery] int page, [FromQuery] int pageSize, [FromQuery] string? keyword = null)
        {
            var response = await _userService.GetUsersAsync(page, pageSize, keyword);
            return StatusCode(response.StatusCode, response);
        }
    }
}
