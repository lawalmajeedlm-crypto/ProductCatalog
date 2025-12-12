using Microsoft.EntityFrameworkCore;
using ProductCatalog.Data;
using ProductCatalog.DTOs;
using ProductCatalog.Entities;
using ProductCatalog.Repositories.Interfaces;

namespace ProductCatalog.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(CatalogDbContext context) : base(context) { }

        public async Task<ApiResponse<IEnumerable<Order>>> GetOrdersByStatusAsync(string status)
        {
            var orders = await _context.Orders
                .Where(o => o.Status == status)
                .AsNoTracking()
                .ToListAsync();

            return orders.Any()
                ? ApiResponse<IEnumerable<Order>>.Ok(orders)
                : ApiResponse<IEnumerable<Order>>.Fail($"No orders found with status '{status}'");
        }

        public async Task<ApiResponse<Order>> GetOrderWithItemsAsync(Guid orderId)
        {
            var order = await _context.Orders
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order is null)
                return ApiResponse<Order>.Fail($"Order with Id {orderId} not found");

            var items = await _context.OrderItems
                .Where(oi => oi.OrderId == orderId)
                .AsNoTracking()
                .ToListAsync();

            return ApiResponse<Order>.Ok(order, "Order retrieved successfully");
        }
    }
}
