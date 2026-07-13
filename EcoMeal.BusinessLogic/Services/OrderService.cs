using EcoMeal.DataAccess.Repositories;
using EcoMeal.DataAccess.Entities;
using EcoMeal.BusinessLogic.Services.Interfaces;
using EcoMeal.Shared.DTOs.OrderDTOs;
using EcoMeal.Shared.DTOs.OrderPackageDTOs;
namespace EcoMeal.BusinessLogic.Services;

public class OrderService(IRepository<Order> orderRepository, IRepository<OrderPackage> orderPackageRepository,
IRepository<Business> businessRepository, IRepository<Package> packageRepository) : IOrderService
{
    public async Task<List<OrderGetDTO>> GetAllOrdersAsync()
    {
        var orders = await orderRepository.GetAllAsync();
        var orderPackages = await orderPackageRepository.GetAllAsync();
        var businesses = await businessRepository.GetAllAsync();
        var packages = await packageRepository.GetAllAsync();

        var list = new List<OrderGetDTO>();
        foreach (var order in orders)
        {
            var dto = MaptoOrderGetDTO(order);
            var business = businesses.FirstOrDefault(b => b.Id == order.BusinessId);
            dto.BusinessName = business?.Name ?? string.Empty;
            dto.BusinessAddress = business?.Address ?? string.Empty;
            dto.StatusName = GetStatusName(order.StatusId);
            dto.OrderPackages = orderPackages.Where(op => op.OrderId == order.Id)
                .Select(op =>
                {
                    var package = packages.FirstOrDefault(p => p.Id == op.PackageId);
                    return new OrderPackageGetDTO
                    {
                        PackageId = op.PackageId,
                        PackageName = package?.Name ?? string.Empty,
                        PackagePrice = package?.Price ?? 0,
                        Quantity = op.Quantity,
                        PickupStart = package?.PickupStart ?? DateTime.MinValue,
                        PickupEnd = package?.PickupEnd ?? DateTime.MinValue
                    };
                }).ToList();
            list.Add(dto);
        }
        return list;
    }
    public async Task<OrderGetDTO> AddOrderAsync(OrderCreateDTO orderCreateDTO)
    {
        var pendingStatusId = Guid.Parse("E2712CD8-CD80-4F27-9711-C676F84E339C");
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

                var package = await packageRepository.GetByIdAsync(orderPackageDTO.PackageId);
                if (package != null)
                {
                    package.Quantity -= orderPackageDTO.Quantity;
                    if (package.Quantity < 0) package.Quantity = 0;
                    await packageRepository.UpdateAsync(package);
                }
            }
        }
        return MaptoOrderGetDTO(addedOrder);
    }

    public async Task<OrderGetDTO> UpdateOrderAsync(Guid id, OrderUpdateDTO orderUpdateDTO)
    {
        var order = await orderRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException($"Order with ID {id} not found.");

        var oldStatusId = order.StatusId;
        var cancelledStatusId = Guid.Parse("224f9b84-e878-4b04-bfb9-6bc6b3eba8dd");

        order.UserId = orderUpdateDTO.UserId;
        order.BusinessId = orderUpdateDTO.BusinessId;
        order.StatusId = orderUpdateDTO.StatusId;
        order.OrderNumber = orderUpdateDTO.OrderNumber;

        var updatedOrder = await orderRepository.UpdateAsync(order);

        // Dacă statusul s-a schimbat în Cancelled, restabilim stocul produselor
        if (orderUpdateDTO.StatusId == cancelledStatusId && oldStatusId != cancelledStatusId)
        {
            var orderPackages = await orderPackageRepository.GetAllAsync();
            var packagesToRestore = orderPackages.Where(op => op.OrderId == id).ToList();

            foreach (var op in packagesToRestore)
            {
                var package = await packageRepository.GetByIdAsync(op.PackageId);
                if (package != null)
                {
                    package.Quantity += op.Quantity; // Restabilim stocul
                    await packageRepository.UpdateAsync(package);
                }
            }
        }

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

            var package = await packageRepository.GetByIdAsync(orderPackageDTO.PackageId);
            if (package != null)
            {
                package.Quantity -= orderPackageDTO.Quantity;
                if (package.Quantity < 0) package.Quantity = 0;
                await packageRepository.UpdateAsync(package);
            }
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

    private static string GetStatusName(Guid statusId)
    {
        return statusId.ToString().ToUpper() switch
        {
            "E2712CD8-CD80-4F27-9711-C676F84E339C" => "Pending",
            "45B77ECA-EFE4-4A2E-867D-7EB6170D3703" => "Confirmed",
            "24FEB601-B7FA-4E23-AA03-F121FAD19347" => "Completed",
            "224F9B84-E878-4B04-BFB9-6BC6B3EBA8DD" => "Cancelled",
            _ => "Unknown"
        };
    }
}

