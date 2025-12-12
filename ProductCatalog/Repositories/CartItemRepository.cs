using Microsoft.EntityFrameworkCore;
using ProductCatalog.Data;
using ProductCatalog.DTOs;
using ProductCatalog.Models;
using ProductCatalog.Repositories.Interfaces;

namespace ProductCatalog.Repositories
{
    public class CartItemRepository : GenericRepository<CartItem>, ICartItemRepository
    {

        public CartItemRepository(CatalogDbContext context) : base(context) { }

        public async Task<ApiResponse<IEnumerable<CartItem>>> GetItemsByCartIdAsync(Guid cartId)
        {
            var items = await _context.CartItems
                .Where(ci => ci.CartId == cartId)
                .AsNoTracking()
                .ToListAsync();

            return items.Any()
                ? ApiResponse<IEnumerable<CartItem>>.Ok(items)
                : ApiResponse<IEnumerable<CartItem>>.Fail($"No items found for Cart {cartId}");
        }

        public async Task<ApiResponse<CartItem>> GetCartItemWithProductAsync(Guid cartItemId)
        {
            var item = await _context.CartItems
                .AsNoTracking()
                .FirstOrDefaultAsync(ci => ci.Id == cartItemId);

            if (item is null)
                return ApiResponse<CartItem>.Fail($"CartItem with Id {cartItemId} not found");

            return ApiResponse<CartItem>.Ok(item, "Cart item retrieved successfully");
        }
    }
}
