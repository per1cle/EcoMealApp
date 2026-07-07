namespace EcoMeal.Shared.DTOs.BusinessDTOs;

public class BusinessCreateDTO
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string Address { get; set; } = null!;
    public string? ImageUrl { get; set; }
    public Guid BusinessTypeId { get; set; }


}