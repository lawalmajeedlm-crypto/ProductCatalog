using Microsoft.EntityFrameworkCore;
using ProductCatalog.Data;
using ProductCatalog.DTOs;
using ProductCatalog.Models;
using ProductCatalog.Repositories.Interfaces;

namespace ProductCatalog.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(CatalogDbContext context) : base(context) { }

        public async Task<ApiResponse<User>> GetByFullNameAsync(string fullName)
        {
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.FullName == fullName && !u.IsDeleted);

            return user is not null
                ? ApiResponse<User>.Ok(user, "User retrieved successfully")
                : ApiResponse<User>.Fail($"No user found with name {fullName}");
        }

        public async Task<ApiResponse<User>> GetByRoleAsync(string role)
        {
            var users = await _context.Users
                .AsNoTracking()
                .Where(u => u.Role == role && !u.IsDeleted)
                .ToListAsync();

            return users.Any()
                ? ApiResponse<User>.Ok(users.First(), $"Users with role {role} retrieved successfully")
                : ApiResponse<User>.Fail($"No users found with role {role}");
        }
    }
}
