using EcoMeal.DataAccess.Repositories;
using EcoMeal.DataAccess.Entities;
using EcoMeal.BusinessLogic.Services.Interfaces;
using EcoMeal.Shared.DTOs.OrderDTOs;
using EcoMeal.Shared.DTOs.OrderPackageDTOs;
namespace EcoMeal.BusinessLogic.Services;

public class OrderService(IRepository<Order> orderRepository, IRepository<OrderPackage> orderPackageRepository) : IOrderService
{
    public async Task<List<OrderGetDTO>> GetAllOrdersAsync()
    {
        var orders = await orderRepository.GetAllAsync();
        return orders.Select(MaptoOrderGetDTO).ToList();
    }
    public async Task<OrderGetDTO> AddOrderAsync(OrderCreateDTO orderCreateDTO)
    {
        var pendingStatusId = Guid.Parse("AICI_PUNE_ID-UL_GENERAT_IN_SSMS");
        var order = new Order
        {
            UserId = orderCreateDTO.UserId,
            BusinessId = orderCreateDTO.BusinessId,
            StatusId = pendingStatusId,
            OrderNumber = orderCreateDTO.OrderNumber
        };

        var addedOrder = await orderRepository.AddAsync(order);

        if (orderCreateDTO.OrderPackages != null && orderCreateDTO.OrderPackages.Any())
        {
            foreach (var orderPackageDTO in orderCreateDTO.OrderPackages)
            {
                var orderPackage = new OrderPackage
                {
                    OrderId = addedOrder.Id,
                    PackageId = orderPackageDTO.PackageId,
                    Quantity = orderPackageDTO.Quantity
                };

                await orderPackageRepository.AddAsync(orderPackage);
            }
        }
        return MaptoOrderGetDTO(addedOrder);
    }

    public async Task<OrderGetDTO> UpdateOrderAsync(Guid id, OrderUpdateDTO orderUpdateDTO)
    {
        var order = await orderRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException($"Order with ID {id} not found.");

        order.UserId = orderUpdateDTO.UserId;
        order.BusinessId = orderUpdateDTO.BusinessId;
        order.StatusId = orderUpdateDTO.StatusId;
        order.OrderNumber = orderUpdateDTO.OrderNumber;

        var updatedOrder = await orderRepository.UpdateAsync(order);
        return MaptoOrderGetDTO(updatedOrder);
    }

    public async Task DeleteOrderAsync(Guid id)
    {
        var order = await orderRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException($"Order with ID {id} not found.");
        await orderRepository.DeleteAsync(order);
    }

    public async Task<OrderGetDTO> PlaceOrderAsync(Guid userId, Guid businessId, List<OrderPackageCreateDTO> orderPackages)
    {
        var order = new Order
        {
            UserId = userId,
            BusinessId = businessId,
            StatusId = Guid.Parse("00000000-0000-0000-0000-000000000001"), // Assuming this is the default status ID for a new order
            OrderNumber = new Random().Next(1000, 9999) // Generate a random order number
        };

        var addedOrder = await orderRepository.AddAsync(order);

        foreach (var orderPackageDTO in orderPackages)
        {
            var orderPackage = new OrderPackage
            {
                OrderId = addedOrder.Id,
                PackageId = orderPackageDTO.PackageId,
                Quantity = orderPackageDTO.Quantity
            };

            await orderPackageRepository.AddAsync(orderPackage);
        }

        return MaptoOrderGetDTO(addedOrder);
    }
    private static OrderGetDTO MaptoOrderGetDTO(Order order)
    {
        return new OrderGetDTO
        {
            Id = order.Id,
            UserId = order.UserId,
            BusinessId = order.BusinessId,
            StatusId = order.StatusId,
            OrderNumber = order.OrderNumber
        };
    }
}

