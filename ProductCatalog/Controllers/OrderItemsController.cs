using Microsoft.AspNetCore.Mvc;
using ProductCatalog.DTOs;
using ProductCatalog.Models;
using ProductCatalog.Repositories.Interfaces;

namespace ProductCatalog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderItemsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderItemsController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<OrderItem>>>> GetAll()
            => Ok(await _unitOfWork.OrderItems.GetAllAsync());

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ApiResponse<OrderItem>>> Get(Guid id)
        {
            var result = await _unitOfWork.OrderItems.GetByIdAsync(id);
            if (!result.Success || result.Data is null) return NotFound(result);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<OrderItem>>> Create(OrderItem orderItem)
        {
            await _unitOfWork.OrderItems.AddAsync(orderItem);
            var saveResult = await _unitOfWork.SaveChangesAsync();
            if (!saveResult.Success) return BadRequest(saveResult);
            return Ok(ApiResponse<OrderItem>.Ok(orderItem, "Order item created successfully"));
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ApiResponse<OrderItem>>> Update(Guid id, OrderItem orderItem)
        {
            var existing = await _unitOfWork.OrderItems.GetByIdAsync(id);
            if (!existing.Success || existing.Data is null) return NotFound(existing);

            existing.Data.Quantity = orderItem.Quantity;
            existing.Data.UnitPrice = orderItem.UnitPrice;

            await _unitOfWork.OrderItems.UpdateAsync(existing.Data);
            var saveResult = await _unitOfWork.SaveChangesAsync();
            if (!saveResult.Success) return BadRequest(saveResult);

            return Ok(ApiResponse<OrderItem>.Ok(existing.Data, "Order item updated successfully"));
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(Guid id)
        {
            var existing = await _unitOfWork.OrderItems.GetByIdAsync(id);
            if (!existing.Success || existing.Data is null) return NotFound(existing);

            await _unitOfWork.OrderItems.DeleteAsync(id);
            var saveResult = await _unitOfWork.SaveChangesAsync();
            if (!saveResult.Success) return BadRequest(saveResult);

            return Ok(ApiResponse<bool>.Ok(true, "Order item deleted successfully"));
        }
    }
}
