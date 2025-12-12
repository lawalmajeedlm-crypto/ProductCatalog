using ProductCatalog.DTOs;
using ProductCatalog.Entities;

namespace ProductCatalog.Repositories.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<ApiResponse<IEnumerable<Order>>> GetOrdersByStatusAsync(string status);
        Task<ApiResponse<Order>> GetOrderWithItemsAsync(Guid orderId);
    }
}
