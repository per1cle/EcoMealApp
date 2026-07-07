namespace EcoMeal.DataAccess.Entities;

public class Business
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? Address { get; set; }
    public string? ImageUrl { get; set; }
    public Guid BusinessTypeId { get; set; }
    public BusinessType BusinessType { get; set; } = null!;
    public ICollection<Package> Packages { get; set; } = new List<Package>();
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}