namespace EcoMeal.DataAccess.Entities;

public class BusinessType
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public ICollection<Business> Businesses { get; set; } = new List<Business>();
}