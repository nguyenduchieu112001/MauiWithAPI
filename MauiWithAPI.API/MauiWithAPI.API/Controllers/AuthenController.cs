using MauiWithAPI.API.Services.Authentication;
using MauiWithAPI.API.Services.Authentication.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MauiWithAPI.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthenController : ControllerBase
    {
        private readonly IAuthenService _authenService;
        public AuthenController(IAuthenService authenService)
        {
            _authenService = authenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterRequestDto model, CancellationToken cancellationToken = default)
        {
            var response = await _authenService.Register(model, cancellationToken);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequestDto model, CancellationToken cancellationToken = default)
        {
            var response = await _authenService.Login(model, cancellationToken);
            if (response == null)
            {
                return Unauthorized("Invalid username or password");
            }
            else return Ok(response);
        }

        [Authorize]
        [HttpGet("getCurrentLoginInformations")]
        public async Task<ActionResult<UserDto>> GetCurrentLoginInformations()
        {
            var response = await _authenService.GetCurrentLoginInformations(HttpContext);
            return Ok(response);
        }
    }
}
