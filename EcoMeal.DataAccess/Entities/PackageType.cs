namespace EcoMeal.DataAccess.Entities;

public class PackageType
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public ICollection<Package> Packages { get; set; } = new List<Package>();
}