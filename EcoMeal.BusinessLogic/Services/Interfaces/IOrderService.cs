using EcoMeal.Shared.DTOs.OrderDTOs;
using EcoMeal.Shared.DTOs.OrderPackageDTOs;
namespace EcoMeal.BusinessLogic.Services.Interfaces;

public interface IOrderService
{
    Task<List<OrderGetDTO>> GetAllOrdersAsync();
    Task<OrderGetDTO> AddOrderAsync(OrderCreateDTO orderCreateDTO);
    Task<OrderGetDTO> UpdateOrderAsync(Guid id, OrderUpdateDTO orderUpdateDTO);
    Task DeleteOrderAsync(Guid id);
    Task<OrderGetDTO> PlaceOrderAsync(Guid userId, Guid businessId, List<OrderPackageCreateDTO> orderPackages);
}