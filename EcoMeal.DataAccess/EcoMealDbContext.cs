using EcoMeal.DataAccess.Configurations;
using EcoMeal.DataAccess.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace EcoMeal.DataAccess;

public class EcoMealDbContext : IdentityDbContext<User, Role, Guid>
{
    public EcoMealDbContext(DbContextOptions<EcoMealDbContext> options) : base(options) { }

    public DbSet<User> User => Set<User>();
    public DbSet<Business> Business => Set<Business>();
    public DbSet<BusinessType> BusinessType => Set<BusinessType>();
    public DbSet<Package> Package => Set<Package>();
    public DbSet<PackageType> PackageType => Set<PackageType>();
    public DbSet<Order> Order => Set<Order>();
    public DbSet<OrderPackage> OrderPackage => Set<OrderPackage>();
    public DbSet<Status> Status => Set<Status>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        base.OnModelCreating(modelBuilder);

        var testUserId = Guid.Parse("00000000-0000-0000-0000-000000000001");

        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new BusinessTypeConfiguration());
        modelBuilder.ApplyConfiguration(new StatusConfiguration());
        modelBuilder.ApplyConfiguration(new PackageTypeConfiguration());
        modelBuilder.ApplyConfiguration(new BusinessConfiguration());
        modelBuilder.ApplyConfiguration(new PackageConfiguration());
        modelBuilder.ApplyConfiguration(new OrderConfiguration());
        modelBuilder.ApplyConfiguration(new OrderPackageConfiguration());

    }
}