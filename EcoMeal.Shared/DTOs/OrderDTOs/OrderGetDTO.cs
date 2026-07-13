using EcoMeal.Shared.DTOs.OrderPackageDTOs;

namespace EcoMeal.Shared.DTOs.OrderDTOs;

public class OrderGetDTO
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid BusinessId { get; set; }
    public Guid StatusId { get; set; }
    public int OrderNumber { get; set; }

    public string BusinessName { get; set; } = string.Empty;
    public string BusinessAddress { get; set; } = string.Empty;
    public string StatusName { get; set; } = string.Empty;
    public List<OrderPackageGetDTO> OrderPackages { get; set; } = new List<OrderPackageGetDTO>();

}