using ProductCatalog.Data;
using ProductCatalog.DTOs;
using ProductCatalog.Models;
using ProductCatalog.Repositories.Interfaces;

namespace ProductCatalog.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CatalogDbContext _context;

        public IProductRepository Products { get; }
        public IOrderRepository Orders { get; }
        public IOrderItemRepository OrderItems { get; }
        public ICartRepository Carts { get; }
        public ICartItemRepository CartItems { get; }
        public IUserRepository Users { get; }

        public UnitOfWork(
            CatalogDbContext context,
            IProductRepository products,
            IOrderRepository orders,
            IOrderItemRepository orderItems,
            ICartRepository carts,
            ICartItemRepository cartItems,
            IUserRepository users)
        {
            _context = context;
            Products = products;
            Orders = orders;
            OrderItems = orderItems;
            Carts = carts;
            CartItems = cartItems;
            Users = users;
        }

        public async Task<ApiResponse<bool>> SaveChangesAsync()
        {
            var changes = await _context.SaveChangesAsync();
            return changes > 0
                ? ApiResponse<bool>.Ok(true, "Changes saved successfully")
                : ApiResponse<bool>.Fail("No changes were saved");
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IGenericRepository<T> Repository<T>() where T : BaseEntity
        {
            return new GenericRepository<T>(_context);
        }
    }
}
