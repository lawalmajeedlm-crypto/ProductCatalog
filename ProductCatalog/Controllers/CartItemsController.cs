using Microsoft.AspNetCore.Mvc;
using ProductCatalog.DTOs;
using ProductCatalog.Models;
using ProductCatalog.Repositories.Interfaces;

namespace ProductCatalog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartItemsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartItemsController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<CartItem>>>> GetAll()
            => Ok(await _unitOfWork.CartItems.GetAllAsync());

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ApiResponse<CartItem>>> Get(Guid id)
        {
            var result = await _unitOfWork.CartItems.GetByIdAsync(id);
            if (!result.Success || result.Data is null) return NotFound(result);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<CartItem>>> Create(CartItem cartItem)
        {
            await _unitOfWork.CartItems.AddAsync(cartItem);
            var saveResult = await _unitOfWork.SaveChangesAsync();
            if (!saveResult.Success) return BadRequest(saveResult);
            return Ok(ApiResponse<CartItem>.Ok(cartItem, "Cart item created successfully"));
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ApiResponse<CartItem>>> Update(Guid id, CartItem cartItem)
        {
            var existing = await _unitOfWork.CartItems.GetByIdAsync(id);
            if (!existing.Success || existing.Data is null) return NotFound(existing);

            existing.Data.Quantity = cartItem.Quantity;

            await _unitOfWork.CartItems.UpdateAsync(existing.Data);
            var saveResult = await _unitOfWork.SaveChangesAsync();
            if (!saveResult.Success) return BadRequest(saveResult);

            return Ok(ApiResponse<CartItem>.Ok(existing.Data, "Cart item updated successfully"));
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(Guid id)
        {
            var existing = await _unitOfWork.CartItems.GetByIdAsync(id);
            if (!existing.Success || existing.Data is null) return NotFound(existing);

            await _unitOfWork.CartItems.DeleteAsync(id);
            var saveResult = await _unitOfWork.SaveChangesAsync();
            if (!saveResult.Success) return BadRequest(saveResult);

            return Ok(ApiResponse<bool>.Ok(true, "Cart item deleted successfully"));
        }
    }
}
