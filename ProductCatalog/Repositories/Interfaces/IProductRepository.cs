using ProductCatalog.Data;
using ProductCatalog.DTOs;
using ProductCatalog.Entities;
using System.Threading.Tasks;

namespace ProductCatalog.Repositories.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
  
        Task<ApiResponse<IEnumerable<Product>>> GetProductsInStockAsync();
        Task<ApiResponse<IEnumerable<Product>>> SearchByNameAsync(string keyword);
        Task<ApiResponse<IEnumerable<Product>>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice);
    }
}
