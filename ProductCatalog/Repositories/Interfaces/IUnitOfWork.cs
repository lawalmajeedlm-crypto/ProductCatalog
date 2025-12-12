using ProductCatalog.DTOs;
using ProductCatalog.Entities;
using ProductCatalog.Models;

namespace ProductCatalog.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        IOrderRepository Orders { get; }
        IOrderItemRepository OrderItems { get; }
        ICartRepository Carts { get; }
        ICartItemRepository CartItems { get; }
        IUserRepository Users { get; }

        IGenericRepository<T> Repository<T>() where T : BaseEntity;

        Task<ApiResponse<bool>> SaveChangesAsync();
    }
}
