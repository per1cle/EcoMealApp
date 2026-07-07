namespace EcoMeal.Shared.DTOs.PackageDTOs;

public class PackageUpdateDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public Guid BusinessId { get; set; }
    public Guid PackageTypeId { get; set; }
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public DateTime PickupStart { get; set; }
    public DateTime PickupEnd { get; set; }
    public string ImageUrl { get; set; } = null!;
}