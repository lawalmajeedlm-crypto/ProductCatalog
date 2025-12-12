using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProductCatalog.Models;
using ProductCatalog.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProductCatalog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<UserDto>>> Register([FromBody] RegisterRequest request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
                return BadRequest(ApiResponse<UserDto>.Fail("Email already registered"));

            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = request.Email,
                Email = request.Email,
                FullName = request.FullName,
                Role = "Customer",
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
                return BadRequest(ApiResponse<UserDto>.Fail(string.Join(", ", result.Errors.Select(e => e.Description))));

            var dto = new UserDto(user.Id, user.Email, user.FullName, user.Role);
            return Ok(ApiResponse<UserDto>.Ok(dto, "User registered successfully"));
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<LoginResponse>>> Login([FromBody] LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return Unauthorized(ApiResponse<LoginResponse>.Fail("Invalid credentials"));

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!result.Succeeded)
                return Unauthorized(ApiResponse<LoginResponse>.Fail("Invalid credentials"));

            var jwtKey = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(jwtKey))
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<LoginResponse>.Fail("JWT Key is missing in configuration"));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("role", user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            var loginResponse = new LoginResponse(jwt, "dummy-refresh-token", DateTime.UtcNow.AddHours(1));

            user.LastLoginAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            return Ok(ApiResponse<LoginResponse>.Ok(loginResponse, "Login successful"));
        }

        [HttpPost("logout")]
        public ActionResult<ApiResponse<bool>> Logout([FromBody] LogoutRequest request)
        {
            return Ok(ApiResponse<bool>.Ok(true, "User logged out successfully"));
        }
    }
}
