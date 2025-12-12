using ProductCatalog.DTOs;
using ProductCatalog.Models;

namespace ProductCatalog.Repositories.Interfaces
{
    public interface IOrderItemRepository : IGenericRepository<OrderItem>
    {
        Task<ApiResponse<IEnumerable<OrderItem>>> GetItemsByOrderIdAsync(Guid orderId);
        Task<ApiResponse<OrderItem>> GetOrderItemWithProductAsync(Guid orderItemId);
    }
}
