using ProductCatalog.DTOs;
using ProductCatalog.Models;

namespace ProductCatalog.Repositories.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<ApiResponse<User>> GetByEmailAsync(string email);
    }
}
