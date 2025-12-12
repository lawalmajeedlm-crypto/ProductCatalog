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

        public async Task<ApiResponse<User>> GetByEmailAsync(string email)
        {
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted);

            return user is not null
                ? ApiResponse<User>.Ok(user, "User retrieved successfully")
                : ApiResponse<User>.Fail($"No user found with email {email}");
        }

        public async Task<ApiResponse<User>> AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return ApiResponse<User>.Ok(user, "User added successfully");
        }

        public async Task<ApiResponse<User>> UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return ApiResponse<User>.Ok(user, "User updated successfully");
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(Guid id, string deletedBy)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                return ApiResponse<bool>.Fail("User not found");

            user.IsDeleted = true;
            user.DeletedAt = DateTime.UtcNow;
            user.DeletedBy = deletedBy;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return ApiResponse<bool>.Ok(true, "User soft deleted successfully");
        }
    }
}
