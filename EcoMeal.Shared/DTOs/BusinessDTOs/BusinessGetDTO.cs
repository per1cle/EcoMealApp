namespace EcoMeal.Shared.DTOs.BusinessDTOs;

public class BusinessGetDTO
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public Guid BusinessTypeId { get; set; }
}

