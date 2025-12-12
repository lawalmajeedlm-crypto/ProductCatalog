using Microsoft.AspNetCore.Mvc;
using ProductCatalog.DTOs;
using ProductCatalog.Models;
using ProductCatalog.Repositories.Interfaces;

namespace ProductCatalog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartsController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<Cart>>>> GetAll()
            => Ok(await _unitOfWork.Carts.GetAllAsync());

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ApiResponse<Cart>>> Get(Guid id)
        {
            var result = await _unitOfWork.Carts.GetByIdAsync(id);
            if (!result.Success || result.Data is null) return NotFound(result);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<Cart>>> Create(Cart cart)
        {
            await _unitOfWork.Carts.AddAsync(cart);
            var saveResult = await _unitOfWork.SaveChangesAsync();
            if (!saveResult.Success) return BadRequest(saveResult);
            return Ok(ApiResponse<Cart>.Ok(cart, "Cart created successfully"));
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ApiResponse<Cart>>> Update(Guid id, Cart cart)
        {
            var existing = await _unitOfWork.Carts.GetByIdAsync(id);
            if (!existing.Success || existing.Data is null) return NotFound(existing);

            existing.Data.UserId = cart.UserId;

            await _unitOfWork.Carts.UpdateAsync(existing.Data);
            var saveResult = await _unitOfWork.SaveChangesAsync();
            if (!saveResult.Success) return BadRequest(saveResult);

            return Ok(ApiResponse<Cart>.Ok(existing.Data, "Cart updated successfully"));
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(Guid id)
        {
            var existing = await _unitOfWork.Carts.GetByIdAsync(id);
            if (!existing.Success || existing.Data is null) return NotFound(existing);

            await _unitOfWork.Carts.DeleteAsync(id);
            var saveResult = await _unitOfWork.SaveChangesAsync();
            if (!saveResult.Success) return BadRequest(saveResult);

            return Ok(ApiResponse<bool>.Ok(true, "Cart deleted successfully"));
        }
    }
}
