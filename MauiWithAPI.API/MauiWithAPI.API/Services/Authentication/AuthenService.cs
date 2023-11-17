using MauiWithAPI.API.Models;
using MauiWithAPI.API.Services.Authentication.Dto;
using MauiWithAPI.API.Services.Users.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace MauiWithAPI.API.Services.Authentication
{
    public class AuthenService : IAuthenService
    {
        private readonly UserManager<IdentityUser<int>> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly IConfiguration _configuration;
        public AuthenService(
            UserManager<IdentityUser<int>> userManager,
            RoleManager<IdentityRole<int>> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public Task<UserDto> GetCurrentLoginInformations(HttpContext httpContext)
        {
            var role = httpContext.User.FindFirstValue(ClaimTypes.Role);
            var roleDto = JsonSerializer.Deserialize<RoleDto>(role);
            var user = new UserDto
            {
                Id = int.Parse(httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)),
                Username = httpContext.User.FindFirstValue(ClaimTypes.Name),
                Email = httpContext.User.FindFirstValue(ClaimTypes.Email),
                Role = roleDto,
            };
            return Task.FromResult(user);
        }

        public async Task<AuthResponse?> Login(LoginRequestDto loginRequestDto, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByNameAsync(loginRequestDto.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, loginRequestDto.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email)
                };

                foreach (var userRole in userRoles)
                {
                    var role = await _roleManager.FindByNameAsync(userRole);
                    var roleDto = new RoleDto
                    {
                        Id = role.Id,
                        RoleName = role.Name,
                    };
                    authClaims.Add(new Claim(ClaimTypes.Role, JsonSerializer.Serialize(roleDto)));
                }

                var token = GetToken(authClaims);
                var authResponse = new AuthResponse
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                };
                return authResponse;
            }
            return null;
        }

        public async Task<ApiResponse<string>> Register(RegisterRequestDto registerRequestDto, CancellationToken cancellationToken = default)
        {
            var username = registerRequestDto.Username;
            if (string.IsNullOrWhiteSpace(username) || username.Length < 6)
            {
                return ApiResponse<string>.Failure("Username must be at least 6 characters", 400);
            }
            else
            {

                var userExists = await _userManager.FindByNameAsync(username);
                if (userExists != null)
                {
                    return ApiResponse<string>.Failure("Username already exists", 409);
                }

                IdentityUser<int> user = new()
                {
                    Email = registerRequestDto.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = registerRequestDto.Username
                };
                var result = await _userManager.CreateAsync(user, registerRequestDto.Password);
                if (!result.Succeeded)
                {
                    var error = result.Errors.FirstOrDefault();
                    return ApiResponse<string>.Failure(error!.Description, 400);
                }

                if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                    await _roleManager.CreateAsync(new IdentityRole<int>(UserRoles.Admin));
                //if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                //    await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
                {
                    await _userManager.AddToRoleAsync(user, UserRoles.Admin);
                }
                //if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
                //{
                //    await _userManager.AddToRoleAsync(user, UserRoles.User);
                //}
            }
            return ApiResponse<string>.Success(200);
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.UtcNow.AddMinutes(30),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
    }
}
