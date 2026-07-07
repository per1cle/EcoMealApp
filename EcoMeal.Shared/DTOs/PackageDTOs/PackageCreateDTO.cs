namespace EcoMeal.Shared.DTOs.PackageDTOs;

public class PackageCreateDTO
{
    public string Name { get; set; } = null!;
    public Guid BusinessId { get; set; }
    public Guid PackageTypeId { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public DateTime PickupStart { get; set; }
    public DateTime PickupEnd { get; set; }
    public string? ImageUrl { get; set; }
}