using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Models;
using ProductCatalog.DTOs;

namespace ProductCatalog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        public UsersController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("by-email/{email}")]
        public async Task<ActionResult<ApiResponse<UserDto>>> GetByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return NotFound(ApiResponse<UserDto>.Fail("User not found"));

            var dto = new UserDto(user.Id, user.Email!, user.FullName, user.Role);
            return Ok(ApiResponse<UserDto>.Ok(dto, "User retrieved successfully"));
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ApiResponse<UserDto>>> GetById(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return NotFound(ApiResponse<UserDto>.Fail("User not found"));

            var dto = new UserDto(user.Id, user.Email!, user.FullName, user.Role);
            return Ok(ApiResponse<UserDto>.Ok(dto, "User retrieved successfully"));
        }

        [HttpPost("add")]
        public async Task<ActionResult<ApiResponse<UserDto>>> AddUser([FromBody] RegisterRequest request)
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

            var dto = new UserDto(user.Id, user.Email!, user.FullName, user.Role);
            return Ok(ApiResponse<UserDto>.Ok(dto, "User added successfully"));
        }

        [HttpPut("update/{id:guid}")]
        public async Task<ActionResult<ApiResponse<UserDto>>> UpdateUser(Guid id, [FromBody] UpdateUserRequest request)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return NotFound(ApiResponse<UserDto>.Fail("User not found"));

            user.FullName = request.FullName ?? user.FullName;
            user.Email = request.Email ?? user.Email;
            user.UserName = request.Email ?? user.UserName;
            user.Role = request.Role ?? user.Role;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest(ApiResponse<UserDto>.Fail(string.Join(", ", result.Errors.Select(e => e.Description))));

            var dto = new UserDto(user.Id, user.Email!, user.FullName, user.Role);
            return Ok(ApiResponse<UserDto>.Ok(dto, "User updated successfully"));
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteUser(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return NotFound(ApiResponse<bool>.Fail("User not found"));

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                return BadRequest(ApiResponse<bool>.Fail(string.Join(", ", result.Errors.Select(e => e.Description))));

            return Ok(ApiResponse<bool>.Ok(true, "User deleted successfully"));
        }
    }
}
