using Microsoft.AspNetCore.Mvc;
using ProductCatalog.DTOs;
using ProductCatalog.Entities;
using ProductCatalog.Models;
using ProductCatalog.Repositories.Interfaces;

namespace ProductCatalog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductDto.CreateProductRequest request)
        {
            var product = new Product
            {
                Name = request.Name,
                Description = request.Description ?? string.Empty,
                Price = request.Price,
                StockQuantity = request.StockQuantity
            };

            var result = await _productRepository.AddAsync(product);

            if (!result.Success)
                return BadRequest(result.Message);

            var response = MapToResponse(result.Data);
            return Ok(response);
        }

        private object MapToResponse(bool data)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ProductDto.UpdateProductRequest request)
        {
            var product = new Product
            {
                Id = request.Id,
                Name = request.Name,
                Description = request.Description ?? string.Empty,
                Price = request.Price,
                StockQuantity = request.StockQuantity
            };

            var result = await _productRepository.UpdateAsync(product);

            if (!result.Success)
                return BadRequest(result.Message);

            var response = MapToResponse(result.Data);
            return Ok(response);
        }

        [HttpGet("in-stock")]
        public async Task<IActionResult> GetProductsInStock()
        {
            var result = await _productRepository.GetProductsInStockAsync();
            if (!result.Success)
                return NotFound(result.Message);

            var response = result.Data.Select(MapToResponse);
            return Ok(response);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchByName([FromQuery] string keyword)
        {
            var result = await _productRepository.SearchByNameAsync(keyword);
            if (!result.Success)
                return NotFound(result.Message);

            var response = result.Data.Select(MapToResponse);
            return Ok(response);
        }

        [HttpGet("price-range")]
        public async Task<IActionResult> GetProductsByPriceRange([FromQuery] decimal minPrice, [FromQuery] decimal maxPrice)
        {
            var result = await _productRepository.GetProductsByPriceRangeAsync(minPrice, maxPrice);
            if (!result.Success)
                return NotFound(result.Message);

            var response = result.Data.Select(MapToResponse);
            return Ok(response);
        }

        private static ProductDto.ProductResponse MapToResponse(Product product)
        {
            return new ProductDto.ProductResponse(
                product.Id,
                product.Name,
                product.Description,
                product.Price,
                product.StockQuantity,
                product.CreatedAt,
                product.UpdatedAt,
                product.CreatedBy,
                product.UpdatedBy,
                product.Pictures.Select(p => new ProductDto.PictureDto(
                    p.Id,
                    p.Url,
                    p.AltText,
                    p.MimeType,
                    p.SortOrder
                ))
            );
        }
    }
}
