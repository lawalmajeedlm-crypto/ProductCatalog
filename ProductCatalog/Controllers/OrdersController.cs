using Microsoft.AspNetCore.Mvc;
using ProductCatalog.DTOs;
using ProductCatalog.Entities;
using ProductCatalog.Repositories.Interfaces;

namespace ProductCatalog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrdersController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<Order>>>> GetAll()
            => Ok(await _unitOfWork.Orders.GetAllAsync());

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ApiResponse<Order>>> Get(Guid id)
        {
            var result = await _unitOfWork.Orders.GetByIdAsync(id);
            if (!result.Success || result.Data is null) return NotFound(result);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<Order>>> Create(Order order)
        {
            await _unitOfWork.Orders.AddAsync(order);
            var saveResult = await _unitOfWork.SaveChangesAsync();
            if (!saveResult.Success) return BadRequest(saveResult);
            return Ok(ApiResponse<Order>.Ok(order, "Order created successfully"));
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ApiResponse<Order>>> Update(Guid id, Order order)
        {
            var existing = await _unitOfWork.Orders.GetByIdAsync(id);
            if (!existing.Success || existing.Data is null) return NotFound(existing);

            existing.Data.Status = order.Status;

            await _unitOfWork.Orders.UpdateAsync(existing.Data);
            var saveResult = await _unitOfWork.SaveChangesAsync();
            if (!saveResult.Success) return BadRequest(saveResult);

            return Ok(ApiResponse<Order>.Ok(existing.Data, "Order updated successfully"));
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(Guid id)
        {
            var existing = await _unitOfWork.Orders.GetByIdAsync(id);
            if (!existing.Success || existing.Data is null) return NotFound(existing);

            await _unitOfWork.Orders.DeleteAsync(id);
            var saveResult = await _unitOfWork.SaveChangesAsync();
            if (!saveResult.Success) return BadRequest(saveResult);

            return Ok(ApiResponse<bool>.Ok(true, "Order deleted successfully"));
        }
    }
}
