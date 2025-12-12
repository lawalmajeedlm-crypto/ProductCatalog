using ProductCatalog.DTOs;
using ProductCatalog.Models;

namespace ProductCatalog.Repositories.Interfaces
{
    public interface ICartItemRepository : IGenericRepository<CartItem>
    {
        Task<ApiResponse<IEnumerable<CartItem>>> GetItemsByCartIdAsync(Guid cartId);
        Task<ApiResponse<CartItem>> GetCartItemWithProductAsync(Guid cartItemId);
    }
}
