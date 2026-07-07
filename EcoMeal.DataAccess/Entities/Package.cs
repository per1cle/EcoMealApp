namespace EcoMeal.DataAccess.Entities;

public class Package
{
    public Guid Id { get; set; }
    public Guid BusinessId { get; set; }
    public Business Business { get; set; } = null!;
    public Guid PackageTypeId { get; set; }
    public PackageType PackageType { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public DateTime PickupStart { get; set; }
    public DateTime PickupEnd { get; set; }
    public ICollection<OrderPackage> OrderPackages { get; set; } = new List<OrderPackage>();

}