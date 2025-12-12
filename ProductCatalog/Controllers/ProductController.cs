using Microsoft.AspNetCore.Mvc;
using ProductCatalog.DTOs;
using ProductCatalog.Entities;
using ProductCatalog.Models;
using ProductCatalog.Repositories.Interfaces;

namespace ProductCatalog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Product>>> GetById(Guid id)
        {
            var result = await _unitOfWork.Products.GetByIdAsync(id);
            if (!result.Success) return NotFound(result);

            return Ok(result);
        }

        [HttpGet("all")]
        public async Task<ActionResult<ApiResponse<IEnumerable<Product>>>> GetAll()
        {
            var result = await _unitOfWork.Products.GetAllAsync();
            if (!result.Success) return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<ActionResult<ApiResponse<bool>>> AddProduct([FromBody] Product product)
        {
            var result = await _unitOfWork.Repository<Product>().AddAsync(product);
            if (!result.Success) return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateProduct([FromBody] Product product)
        {
            var result = await _unitOfWork.Repository<Product>().UpdateAsync(product);
            if (!result.Success) return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> SoftDelete(Guid id)
        {
            var result = await _unitOfWork.Repository<Product>().SoftDeleteAsync(id, "system");
            if (!result.Success) return BadRequest(result);

            return Ok(result);
        }
    }
}
