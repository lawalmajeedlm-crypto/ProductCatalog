using Microsoft.EntityFrameworkCore;
using ProductCatalog.Data;
using ProductCatalog.DTOs;
using ProductCatalog.Models;
using ProductCatalog.Repositories.Interfaces;

namespace ProductCatalog.Repositories
{
    public class OrderItemRepository : GenericRepository<OrderItem>, IOrderItemRepository
    {
        public OrderItemRepository(CatalogDbContext context) : base(context) { }

        public async Task<ApiResponse<IEnumerable<OrderItem>>> GetItemsByOrderIdAsync(Guid orderId)
        {
            var items = await _context.OrderItems
                .Where(oi => oi.OrderId == orderId)
                .AsNoTracking()
                .ToListAsync();

            return items.Any()
                ? ApiResponse<IEnumerable<OrderItem>>.Ok(items)
                : ApiResponse<IEnumerable<OrderItem>>.Fail($"No items found for Order {orderId}");
        }

        public async Task<ApiResponse<OrderItem>> GetOrderItemWithProductAsync(Guid orderItemId)
        {
            var item = await _context.OrderItems
                .AsNoTracking()
                .FirstOrDefaultAsync(oi => oi.Id == orderItemId);

            if (item is null)
                return ApiResponse<OrderItem>.Fail($"OrderItem with Id {orderItemId} not found");

            return ApiResponse<OrderItem>.Ok(item, "Order item retrieved successfully");
        }
    }
}
