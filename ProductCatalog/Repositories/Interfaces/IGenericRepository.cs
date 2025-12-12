using ProductCatalog.DTOs;

namespace ProductCatalog.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<ApiResponse<IEnumerable<T>>> GetAllAsync();
        Task<ApiResponse<T>> GetByIdAsync(Guid id);
        Task<ApiResponse<bool>> AddAsync(T entity);
        Task<ApiResponse<bool>> UpdateAsync(T entity);
        Task<ApiResponse<bool>> DeleteAsync(Guid id);
        Task<ApiResponse<bool>> SoftDeleteAsync(Guid id, string deletedBy);
    }
}
