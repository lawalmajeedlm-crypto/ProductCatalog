using ProductCatalog.DTOs;
using ProductCatalog.Models;

namespace ProductCatalog.Repositories.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<ApiResponse<User>> GetByFullNameAsync(string fullName);
        Task<ApiResponse<User>> GetByRoleAsync(string role);
       
    }
}
