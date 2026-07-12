using System.ComponentModel.DataAnnotations;

namespace EcoMeal.Shared.DTOs.OrderDTOs;

public class OrderUpdateDTO
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "User ID is required.")]
    public Guid UserId { get; set; }

    [Required(ErrorMessage = "Business ID is required.")]
    public Guid BusinessId { get; set; }

    [Required(ErrorMessage = "Status ID is required.")]
    public Guid StatusId { get; set; }

    [Required(ErrorMessage = "Order number is required.")]
    public int OrderNumber { get; set; }
}