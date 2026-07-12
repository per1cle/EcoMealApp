namespace EcoMeal.Shared.DTOs.OrderDTOs;

public class OrderGetDTO
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid BusinessId { get; set; }
    public Guid StatusId { get; set; }
    public int OrderNumber { get; set; }
}