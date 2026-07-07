namespace EcoMeal.DataAccess.Entities;

public class Status
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}