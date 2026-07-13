namespace EcoMeal.Shared.DTOs.OrderPackageDTOs;

public class OrderPackageGetDTO
{
    public Guid PackageId { get; set; }
    public string PackageName { get; set; } = string.Empty;
    public decimal PackagePrice { get; set; }
    public int Quantity { get; set; }
    public DateTime PickupStart { get; set; }
    public DateTime PickupEnd { get; set; }
}