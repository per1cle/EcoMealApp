using Microsoft.AspNetCore.Identity;

namespace EcoMeal.DataAccess.Entities;

public class User : IdentityUser<Guid>
{
    public string Name { get; set; } = null!;
    public ICollection<Order> Orders { get; set; } = new List<Order>();

}
