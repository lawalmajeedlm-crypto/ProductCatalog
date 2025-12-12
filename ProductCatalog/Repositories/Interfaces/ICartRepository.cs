using ProductCatalog.DTOs;
using ProductCatalog.Models;

namespace ProductCatalog.Repositories.Interfaces
{
    public interface ICartRepository : IGenericRepository<Cart>
    {
        Task<ApiResponse<Cart>> GetCartWithItemsAsync(Guid cartId);
    }
}
