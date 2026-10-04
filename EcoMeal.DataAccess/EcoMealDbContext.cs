using EcoMeal.DataAccess.Configurations;
using EcoMeal.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
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

        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new BusinessTypeConfiguration());
        modelBuilder.ApplyConfiguration(new StatusConfiguration());
        modelBuilder.ApplyConfiguration(new PackageTypeConfiguration());
        modelBuilder.ApplyConfiguration(new BusinessConfiguration());
        modelBuilder.ApplyConfiguration(new PackageConfiguration());
        modelBuilder.ApplyConfiguration(new OrderConfiguration());
        modelBuilder.ApplyConfiguration(new OrderPackageConfiguration());

        // IDs
        var roleAdminId = Guid.Parse("10000000-0000-0000-0000-000000000001");
        var roleBusinessId = Guid.Parse("10000000-0000-0000-0000-000000000002");
        var roleCustomerId = Guid.Parse("10000000-0000-0000-0000-000000000003");

        var userBistroId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        var userFreshMartId = Guid.Parse("00000000-0000-0000-0000-000000000002");
        var userAdminId = Guid.Parse("00000000-0000-0000-0000-000000000003");

        var businessTypeId1 = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var businessTypeId2 = Guid.Parse("22222222-2222-2222-2222-222222222222");
        var businessTypeId3 = Guid.Parse("33333333-3333-3333-3333-333333333333");

        var packageTypeId1 = Guid.Parse("44444444-4444-4444-4444-444444444444");
        var packageTypeId2 = Guid.Parse("55555555-5555-5555-5555-555555555555");

        var businessId1 = Guid.Parse("66666666-6666-6666-6666-666666666666");
        var businessId2 = Guid.Parse("77777777-7777-7777-7777-777777777777");

        var packageId1 = Guid.Parse("88888888-8888-8888-8888-888888888888");
        var packageId2 = Guid.Parse("99999999-9999-9999-9999-999999999999");

        var statusPendingId = Guid.Parse("E2712CD8-CD80-4F27-9711-C676F84E339C");
        var statusConfirmedId = Guid.Parse("45B77ECA-EFE4-4A2E-867D-7EB6170D3703");
        var statusCompletedId = Guid.Parse("24FEB601-B7FA-4E23-AA03-F121FAD19347");
        var statusCancelledId = Guid.Parse("224F9B84-E878-4B04-BFB9-6BC6B3EBA8DD");

        // Seed Roles
        modelBuilder.Entity<Role>().HasData(
            new Role { Id = roleAdminId, Name = "Admin", NormalizedName = "ADMIN", ConcurrencyStamp = roleAdminId.ToString() },
            new Role { Id = roleBusinessId, Name = "Business", NormalizedName = "BUSINESS", ConcurrencyStamp = roleBusinessId.ToString() },
            new Role { Id = roleCustomerId, Name = "Customer", NormalizedName = "CUSTOMER", ConcurrencyStamp = roleCustomerId.ToString() }
        );

        // Seed Users (Password: Password123!)
        const string passwordHash = "AQAAAAIAAYagAAAAEK6QRy078UZfLCymeFSi8u2/PAgSuWVrg9NHcr/NkHITgrJhS6S4kwlc22j0lVdXIg==";
        var bistroUser = new User
        {
            Id = userBistroId,
            UserName = "bistro@ecomeal.com",
            NormalizedUserName = "BISTRO@ECOMEAL.COM",
            Email = "bistro@ecomeal.com",
            NormalizedEmail = "BISTRO@ECOMEAL.COM",
            Name = "Green Bite Bistro",
            EmailConfirmed = true,
            PasswordHash = passwordHash,
            SecurityStamp = "d87a55be-2bf3-4614-bfe5-dcf46e5b4ff4",
            ConcurrencyStamp = "a7251786-8a7e-4043-9ce6-15f1f9e20a32"
        };

        var freshMartUser = new User
        {
            Id = userFreshMartId,
            UserName = "freshmart@ecomeal.com",
            NormalizedUserName = "FRESHMART@ECOMEAL.COM",
            Email = "freshmart@ecomeal.com",
            NormalizedEmail = "FRESHMART@ECOMEAL.COM",
            Name = "FreshMart",
            EmailConfirmed = true,
            PasswordHash = passwordHash,
            SecurityStamp = "f0896082-9626-4448-96ef-23e5a5a1f6a1",
            ConcurrencyStamp = "b9bf8b58-868d-4f1b-b463-547781b16866"
        };

        var adminUser = new User
        {
            Id = userAdminId,
            UserName = "admin@ecomeal.com",
            NormalizedUserName = "ADMIN@ECOMEAL.COM",
            Email = "admin@ecomeal.com",
            NormalizedEmail = "ADMIN@ECOMEAL.COM",
            Name = "Admin",
            EmailConfirmed = true,
            PasswordHash = passwordHash,
            SecurityStamp = "e98e4f50-3a33-470b-8d07-ee65f6c88f11",
            ConcurrencyStamp = "c5b2e04e-28b7-4db5-b384-95d43b2f5d91"
        };

        modelBuilder.Entity<User>().HasData(bistroUser, freshMartUser, adminUser);

        // Seed User Roles
        modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(
            new IdentityUserRole<Guid> { UserId = userBistroId, RoleId = roleBusinessId },
            new IdentityUserRole<Guid> { UserId = userFreshMartId, RoleId = roleBusinessId },
            new IdentityUserRole<Guid> { UserId = userAdminId, RoleId = roleAdminId }
        );

        // Seed Statuses
        modelBuilder.Entity<Status>().HasData(
            new Status { Id = statusPendingId, Name = "Pending" },
            new Status { Id = statusConfirmedId, Name = "Confirmed" },
            new Status { Id = statusCompletedId, Name = "Completed" },
            new Status { Id = statusCancelledId, Name = "Cancelled" }
        );

        // Seed Business Types
        modelBuilder.Entity<BusinessType>().HasData(
            new BusinessType { Id = businessTypeId1, Name = "Restaurant" },
            new BusinessType { Id = businessTypeId2, Name = "Supermarket" },
            new BusinessType { Id = businessTypeId3, Name = "Bakery" }
        );

        // Seed Package Types
        modelBuilder.Entity<PackageType>().HasData(
            new PackageType { Id = packageTypeId1, Name = "Surprise Bag" },
            new PackageType { Id = packageTypeId2, Name = "Pastry Bag" }
        );

        // Seed Businesses
        modelBuilder.Entity<Business>().HasData(
            new Business
            {
                Id = businessId1,
                UserId = userBistroId,
                Name = "Green Bite Bistro",
                Description = "A cozy place with sustainable and delicious food.",
                Address = "Str. Victoriei, Nr. 10",
                ImageUrl = "https://images.unsplash.com/photo-1517248135467-4c7edcad34c4",
                BusinessTypeId = businessTypeId1,
                IsApproved = true,
                Latitude = 44.3185,
                Longitude = 23.7998
            },
            new Business
            {
                Id = businessId2,
                UserId = userFreshMartId,
                Name = "FreshMart",
                Description = "Your local supermarket for fresh produce and essentials.",
                Address = "Str. Libertății, Nr. 5",
                ImageUrl = "https://images.unsplash.com/photo-1586201375761-83865001e3b6",
                BusinessTypeId = businessTypeId2,
                IsApproved = true,
                Latitude = 44.3235,
                Longitude = 23.8050
            }
        );

        // Seed Packages
        modelBuilder.Entity<Package>().HasData(
            new Package
            {
                Id = packageId1,
                Name = "End of Day Surprise",
                Description = "Delicious leftover meals perfectly fine to eat.",
                Price = 15.50m,
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
                Price = 3.50m,
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