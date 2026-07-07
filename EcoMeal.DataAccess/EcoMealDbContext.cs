using EcoMeal.DataAccess.Configurations;
using EcoMeal.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
namespace EcoMeal.DataAccess;

public class EcoMealDbContext : DbContext
{
    public EcoMealDbContext(DbContextOptions<EcoMealDbContext> options) : base(options) { }

    public DbSet<User> User => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Business> Businesses => Set<Business>();
    public DbSet<BusinessType> BusinessTypes => Set<BusinessType>();
    public DbSet<Package> Packages => Set<Package>();
    public DbSet<PackageType> PackageTypes => Set<PackageType>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderPackage> OrderPackages => Set<OrderPackage>();
    public DbSet<Status> Statuses => Set<Status>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new BusinessTypeConfiguration());
        modelBuilder.ApplyConfiguration(new StatusConfiguration());
        modelBuilder.ApplyConfiguration(new PackageTypeConfiguration());
        modelBuilder.ApplyConfiguration(new BusinessConfiguration());
        modelBuilder.ApplyConfiguration(new PackageConfiguration());
        modelBuilder.ApplyConfiguration(new OrderConfiguration());
        modelBuilder.ApplyConfiguration(new OrderPackageConfiguration());

        var businessTypeId1 = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var businessTypeId2 = Guid.Parse("22222222-2222-2222-2222-222222222222");
        var businessTypeId3 = Guid.Parse("33333333-3333-3333-3333-333333333333");
        var packageTypeId1 = Guid.Parse("44444444-4444-4444-4444-444444444444");
        var packageTypeId2 = Guid.Parse("55555555-5555-5555-5555-555555555555");
        var businessId1 = Guid.Parse("66666666-6666-6666-6666-666666666666");
        var businessId2 = Guid.Parse("77777777-7777-7777-7777-777777777777");
        var packageId1 = Guid.Parse("88888888-8888-8888-8888-888888888888");
        var packageId2 = Guid.Parse("99999999-9999-9999-9999-999999999999");

        modelBuilder.Entity<BusinessType>().HasData(
            new BusinessType { Id = businessTypeId1, Name = "Restaurant" },
            new BusinessType { Id = businessTypeId2, Name = "Supermarket" },
            new BusinessType { Id = businessTypeId3, Name = "Bakery" }
        );

        modelBuilder.Entity<PackageType>().HasData(
            new PackageType { Id = packageTypeId1, Name = "Surprise Bag" },
            new PackageType { Id = packageTypeId2, Name = "Pastry Bag" }
        );

        modelBuilder.Entity<Business>().HasData(
            new Business
            {
                Id = businessId1,
                Name = "Green Bite Bistro",
                Description = "A cozy place with sustainable and delicious food.",
                Address = "Str. Victoriei, Nr. 10",
                ImageUrl = "https://images.unsplash.com/photo-1517248135467-4c7edcad34c4",
                BusinessTypeId = businessTypeId1
            },
            new Business
            {
                Id = businessId2,
                Name = "FreshMart",
                Description = "Your local supermarket for fresh produce and essentials.",
                Address = "Str. Libertății, Nr. 5",
                ImageUrl = "https://images.unsplash.com/photo-1586201375761-83865001e3b6",
                BusinessTypeId = businessTypeId2
            }
        );

        modelBuilder.Entity<Package>().HasData(
            new Package
            {
                Id = packageId1,
                Name = "End of Day Surprise",
                Description = "Delicious leftover meals perfectly fine to eat.",
                Price = (decimal)15.50f,
                Quantity = 5,
                PickupStart = new DateTime(2026, 7, 8, 18, 0, 0),
                PickupEnd = new DateTime(2026, 7, 8, 22, 0, 0),
                ImageUrl = "https://images.unsplash.com/photo-1542838132-92c53300491e",
                BusinessId = businessId1,
                PackageTypeId = packageTypeId1
            },
            new Package
            {
                Id = packageId2,
                Name = "Yesterday's Bread Bundle",
                Description = "Perfect for toast, sandwiches or making breadcrumbs.",
                Price = (decimal)3.50f,
                Quantity = 20,
                PickupStart = new DateTime(2026, 7, 8, 16, 0, 0),
                PickupEnd = new DateTime(2026, 7, 8, 20, 0, 0),
                ImageUrl = "https://images.unsplash.com/photo-1509440159596-0249088772ff",
                BusinessId = businessId2,
                PackageTypeId = packageTypeId2
            }
        );

    }
}