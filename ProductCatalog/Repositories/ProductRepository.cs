using Microsoft.EntityFrameworkCore;
using ProductCatalog.Data;
using ProductCatalog.DTOs;
using ProductCatalog.Entities;
using ProductCatalog.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalog.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(CatalogDbContext context) : base(context) { }

        public async Task<ApiResponse<IEnumerable<Product>>> GetProductsInStockAsync()
        {
            var products = await _context.Products
                .Include(p => p.Pictures) 
                .AsNoTracking()
                .Where(p => p.StockQuantity > 0 && !p.IsDeleted)
                .ToListAsync();

            return products.Any()
                ? ApiResponse<IEnumerable<Product>>.Ok(products, "Products in stock retrieved successfully")
                : ApiResponse<IEnumerable<Product>>.Fail("No products currently in stock");
        }

        public async Task<ApiResponse<IEnumerable<Product>>> SearchByNameAsync(string keyword)
        {
            var products = await _context.Products
                .Include(p => p.Pictures) 
                .AsNoTracking()
                .Where(p => p.Name.Contains(keyword) && !p.IsDeleted)
                .ToListAsync();

            return products.Any()
                ? ApiResponse<IEnumerable<Product>>.Ok(products, $"Products matching '{keyword}' retrieved successfully")
                : ApiResponse<IEnumerable<Product>>.Fail($"No products found matching '{keyword}'");
        }

        public async Task<ApiResponse<IEnumerable<Product>>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            var products = await _context.Products
                .Include(p => p.Pictures) 
                .AsNoTracking()
                .Where(p => p.Price >= minPrice && p.Price <= maxPrice && !p.IsDeleted)
                .ToListAsync();

            return products.Any()
                ? ApiResponse<IEnumerable<Product>>.Ok(products, $"Products between {minPrice:C} and {maxPrice:C} retrieved successfully")
                : ApiResponse<IEnumerable<Product>>.Fail($"No products found in the price range {minPrice:C} - {maxPrice:C}");
        }
    }
}
