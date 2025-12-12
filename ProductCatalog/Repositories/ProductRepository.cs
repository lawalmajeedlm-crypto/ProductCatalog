using Microsoft.EntityFrameworkCore;
using ProductCatalog.Data;
using ProductCatalog.DTOs;
using ProductCatalog.Entities;
using ProductCatalog.Repositories.Interfaces;

namespace ProductCatalog.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(CatalogDbContext context) : base(context) { }

        public async Task<ApiResponse<IEnumerable<Product>>> GetProductsInStockAsync()
        {
            var products = await _context.Products
                .Where(p => p.StockQuantity > 0 && !p.IsDeleted)
                .AsNoTracking()
                .ToListAsync();

            return products.Any()
                ? ApiResponse<IEnumerable<Product>>.Ok(products)
                : ApiResponse<IEnumerable<Product>>.Fail("No products in stock");
        }

        public async Task<ApiResponse<IEnumerable<Product>>> SearchByNameAsync(string keyword)
        {
            var products = await _context.Products
                .Where(p => p.Name.Contains(keyword) && !p.IsDeleted)
                .AsNoTracking()
                .ToListAsync();

            return products.Any()
                ? ApiResponse<IEnumerable<Product>>.Ok(products)
                : ApiResponse<IEnumerable<Product>>.Fail("No products found matching search keyword");
        }

        public async Task<ApiResponse<IEnumerable<Product>>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            var products = await _context.Products
                .Where(p => p.Price >= minPrice && p.Price <= maxPrice && !p.IsDeleted)
                .AsNoTracking()
                .ToListAsync();

            return products.Any()
                ? ApiResponse<IEnumerable<Product>>.Ok(products)
                : ApiResponse<IEnumerable<Product>>.Fail($"No products found in price range {minPrice} - {maxPrice}");
        }
    }
}
