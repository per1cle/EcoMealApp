using EcoMeal.DataAccess.Repositories;
using EcoMeal.DataAccess.Entities;
using EcoMeal.BusinessLogic.Services.Interfaces;
using EcoMeal.Shared.DTOs.OrderDTOs;
using EcoMeal.Shared.DTOs.OrderPackageDTOs;
namespace EcoMeal.BusinessLogic.Services;

public class OrderService(
    IRepository<Order> orderRepository,
    IRepository<OrderPackage> orderPackageRepository,
    IRepository<Business> businessRepository,
    IRepository<Package> packageRepository,
    Microsoft.AspNetCore.Identity.UserManager<User> userManager,
    IEmailService emailService) : IOrderService
{
    private static readonly Guid PendingStatusId = Guid.Parse("E2712CD8-CD80-4F27-9711-C676F84E339C");
    private static readonly Guid ConfirmedStatusId = Guid.Parse("45B77ECA-EFE4-4A2E-867D-7EB6170D3703");
    private static readonly Guid CompletedStatusId = Guid.Parse("24FEB601-B7FA-4E23-AA03-F121FAD19347");
    private static readonly Guid CancelledStatusId = Guid.Parse("224F9B84-E878-4B04-BFB9-6BC6B3EBA8DD");

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
        if (orderCreateDTO.OrderPackages == null || !orderCreateDTO.OrderPackages.Any())
        {
            throw new InvalidOperationException("An order must contain at least one package.");
        }

        var business = await businessRepository.GetByIdAsync(orderCreateDTO.BusinessId)
            ?? throw new KeyNotFoundException($"Business with ID {orderCreateDTO.BusinessId} not found.");

        // Validate all packages belong to the specified business and have enough quantity
        var packagesToUpdate = new List<(Package Package, int Quantity)>();
        foreach (var orderPackageDTO in orderCreateDTO.OrderPackages)
        {
            if (orderPackageDTO.Quantity <= 0)
            {
                throw new InvalidOperationException("Package quantity must be greater than zero.");
            }

            var package = await packageRepository.GetByIdAsync(orderPackageDTO.PackageId)
                ?? throw new KeyNotFoundException($"Package with ID {orderPackageDTO.PackageId} not found.");

            if (package.BusinessId != orderCreateDTO.BusinessId)
            {
                throw new InvalidOperationException($"Package '{package.Name}' does not belong to business '{business.Name}'. An order cannot contain packages from different restaurants.");
            }

            if (package.Quantity < orderPackageDTO.Quantity)
            {
                throw new InvalidOperationException($"Not enough stock for package '{package.Name}'. Available: {package.Quantity}, requested: {orderPackageDTO.Quantity}.");
            }

            packagesToUpdate.Add((package, orderPackageDTO.Quantity));
        }

        var order = new Order
        {
            UserId = orderCreateDTO.UserId,
            BusinessId = orderCreateDTO.BusinessId,
            StatusId = PendingStatusId,
            OrderNumber = orderCreateDTO.OrderNumber
        };

        var addedOrder = await orderRepository.AddAsync(order);

        foreach (var (package, quantity) in packagesToUpdate)
        {
            var orderPackage = new OrderPackage
            {
                OrderId = addedOrder.Id,
                PackageId = package.Id,
                Quantity = quantity
            };

            await orderPackageRepository.AddAsync(orderPackage);

            package.Quantity -= quantity;
            await packageRepository.UpdateAsync(package);
        }

        return MaptoOrderGetDTO(addedOrder);
    }

    public async Task<OrderGetDTO> UpdateOrderAsync(Guid id, OrderUpdateDTO orderUpdateDTO)
    {
        var order = await orderRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException($"Order with ID {id} not found.");

        var oldStatusId = order.StatusId;
        var newStatusId = orderUpdateDTO.StatusId;

        order.UserId = orderUpdateDTO.UserId;
        order.BusinessId = orderUpdateDTO.BusinessId;
        order.StatusId = orderUpdateDTO.StatusId;
        order.OrderNumber = orderUpdateDTO.OrderNumber;

        var updatedOrder = await orderRepository.UpdateAsync(order);

        // If status changed to Cancelled, restore package stock
        if (newStatusId == CancelledStatusId && oldStatusId != CancelledStatusId)
        {
            var orderPackages = await orderPackageRepository.GetAllAsync();
            var packagesToRestore = orderPackages.Where(op => op.OrderId == id).ToList();

            foreach (var op in packagesToRestore)
            {
                var package = await packageRepository.GetByIdAsync(op.PackageId);
                if (package != null)
                {
                    package.Quantity += op.Quantity;
                    await packageRepository.UpdateAsync(package);
                }
            }
        }

        // Trigger emails if status changed
        if (newStatusId != oldStatusId)
        {
            try
            {
                var customer = await userManager.FindByIdAsync(order.UserId.ToString());
                var business = await businessRepository.GetByIdAsync(order.BusinessId);

                if (customer != null && !string.IsNullOrWhiteSpace(customer.Email) && business != null)
                {
                    var customerName = !string.IsNullOrWhiteSpace(customer.Name)
                        ? customer.Name
                        : customer.UserName ?? "Client";

                    var allOrderPackages = await orderPackageRepository.GetAllAsync();
                    var thisOrderPackages = allOrderPackages.Where(op => op.OrderId == order.Id).ToList();
                    var allPackages = await packageRepository.GetAllAsync();

                    var itemsList = thisOrderPackages.Select(op =>
                    {
                        var pkg = allPackages.FirstOrDefault(p => p.Id == op.PackageId);
                        return (
                            PackageName: pkg?.Name ?? "EcoMeal Package",
                            Quantity: op.Quantity,
                            Price: pkg?.Price ?? 0m
                        );
                    }).ToList();

                    var totalAmount = itemsList.Sum(i => i.Price * i.Quantity);

                    if (newStatusId == ConfirmedStatusId)
                    {
                        await emailService.SendOrderConfirmedEmailAsync(
                            customer.Email,
                            customerName,
                            order.OrderNumber,
                            business.Name,
                            business.Address ?? string.Empty,
                            business.Latitude,
                            business.Longitude,
                            totalAmount,
                            itemsList
                        );
                    }
                    else if (newStatusId == CompletedStatusId)
                    {
                        await emailService.SendOrderCompletedEmailAsync(
                            customer.Email,
                            customerName,
                            order.OrderNumber,
                            business.Name,
                            business.Address ?? string.Empty,
                            totalAmount
                        );
                    }
                    else if (newStatusId == CancelledStatusId)
                    {
                        await emailService.SendOrderCancelledEmailAsync(
                            customer.Email,
                            customerName,
                            order.OrderNumber,
                            business.Name
                        );
                    }
                }
            }
            catch
            {
                // Email delivery failure should not break status update
            }
        }

        return MaptoOrderGetDTO(updatedOrder);
    }

    public async Task DeleteOrderAsync(Guid id)
    {
        var order = await orderRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException($"Order with ID {id} not found.");

        var allOrderPackages = await orderPackageRepository.GetAllAsync();
        var packagesToDelete = allOrderPackages.Where(op => op.OrderId == id).ToList();

        // Restore package stock if order was not cancelled or completed
        if (order.StatusId != CancelledStatusId && order.StatusId != CompletedStatusId)
        {
            foreach (var op in packagesToDelete)
            {
                var package = await packageRepository.GetByIdAsync(op.PackageId);
                if (package != null)
                {
                    package.Quantity += op.Quantity;
                    await packageRepository.UpdateAsync(package);
                }
            }
        }

        foreach (var op in packagesToDelete)
        {
            await orderPackageRepository.DeleteAsync(op);
        }

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

