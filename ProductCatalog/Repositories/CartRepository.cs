using Microsoft.EntityFrameworkCore;
using ProductCatalog.Data;
using ProductCatalog.DTOs;
using ProductCatalog.Models;
using ProductCatalog.Repositories.Interfaces;

namespace ProductCatalog.Repositories
{
    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {

        public CartRepository(CatalogDbContext context) : base(context) { }

        public async Task<ApiResponse<Cart>> GetCartWithItemsAsync(Guid cartId)
        {
            var cart = await _context.Carts
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == cartId);

            if (cart is null)
                return ApiResponse<Cart>.Fail($"Cart with Id {cartId} not found");

            var items = await _context.CartItems
                .Where(ci => ci.CartId == cartId)
                .AsNoTracking()
                .ToListAsync();

            return ApiResponse<Cart>.Ok(cart, "Cart retrieved successfully");
        }
    }
}

