namespace EcoMeal.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using EcoMeal.Shared.DTOs.OrderDTOs;
using EcoMeal.BusinessLogic.Services.Interfaces;

[ApiController]
[Route("api/orders")]
public class OrderController(IOrderService orderService) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<List<OrderGetDTO>>> GetOrders()
    {
        try
        {
            var orders = await orderService.GetAllOrdersAsync();
            return Ok(orders);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while retrieving orders: " + ex.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<OrderGetDTO>> AddOrder(OrderCreateDTO orderCreateDTO)
    {
        try
        {
            var order = await orderService.AddOrderAsync(orderCreateDTO);
            return CreatedAtAction(nameof(GetOrders), new { id = order.Id }, order);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while adding the order: " + ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<OrderGetDTO>> UpdateOrder(Guid id, OrderUpdateDTO orderUpdateDTO)
    {
        try
        {
            var order = await orderService.UpdateOrderAsync(id, orderUpdateDTO);
            return Ok(order);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while updating the order: " + ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteOrder(Guid id)
    {
        try
        {
            await orderService.DeleteOrderAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }
}