using ProductCatalog.Models;

namespace ProductCatalog.Abstractions
{
    public interface IOrderRepository
    {
        Task AddAsync(Order order, CancellationToken ct);
        Task<Order?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<List<Order>> GetAllAsync(CancellationToken ct);
        Task UpdateAsync(Order order, CancellationToken ct);
        Task SoftDeleteAsync(Order order, string deletedBy, CancellationToken ct);
    }
}