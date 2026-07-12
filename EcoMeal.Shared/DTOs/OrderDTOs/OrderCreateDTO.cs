using System.ComponentModel.DataAnnotations;
using EcoMeal.Shared.DTOs.OrderPackageDTOs;

namespace EcoMeal.Shared.DTOs.OrderDTOs;

public class OrderCreateDTO
{
    [Required(ErrorMessage = "User ID is required.")]
    public Guid UserId { get; set; }

    [Required(ErrorMessage = "Business ID is required.")]
    public Guid BusinessId { get; set; }

    [Required(ErrorMessage = "Order number is required.")]
    public int OrderNumber { get; set; }

    public List<OrderPackageCreateDTO> OrderPackages { get; set; } = new List<OrderPackageCreateDTO>();
}