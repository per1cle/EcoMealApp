namespace EcoMeal.Shared.DTOs.OrderPackageDTOs;

public class OrderPackageCreateDTO
{
    public Guid PackageId { get; set; }
    public int Quantity { get; set; }
}