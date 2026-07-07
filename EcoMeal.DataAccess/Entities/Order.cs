namespace EcoMeal.DataAccess.Entities;

public class Order
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public Guid BusinessId { get; set; }
    public Business Business { get; set; } = null!;
    public Guid StatusId { get; set; }
    public Status Status { get; set; } = null!;
    public int OrderNumber { get; set; }
    public ICollection<OrderPackage> OrderPackages { get; set; } = new List<OrderPackage>();

}
