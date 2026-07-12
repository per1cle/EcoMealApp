using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EcoMeal.DataAccess.Entities;
namespace EcoMeal.DataAccess.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(100);
        }
    }

    public class BusinessTypeConfiguration : IEntityTypeConfiguration<BusinessType>
    {
        public void Configure(EntityTypeBuilder<BusinessType> builder)
        {
            builder.ToTable("BusinessType");
            builder.HasKey(bt => bt.Id);
            builder.Property(bt => bt.Name)
                .IsRequired()
                .HasMaxLength(100);
        }
    }

    public class StatusConfiguration : IEntityTypeConfiguration<Status>
    {
        public void Configure(EntityTypeBuilder<Status> builder)
        {
            builder.ToTable("Status");
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);
        }
    }

    public class PackageTypeConfiguration : IEntityTypeConfiguration<PackageType>
    {
        public void Configure(EntityTypeBuilder<PackageType> builder)
        {
            builder.ToTable("PackageType");
            builder.HasKey(pt => pt.Id);
            builder.Property(pt => pt.Name)
                .IsRequired()
                .HasMaxLength(100);
        }
    }

    public class BusinessConfiguration : IEntityTypeConfiguration<Business>
    {
        public void Configure(EntityTypeBuilder<Business> builder)
        {
            builder.ToTable("Business");
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(255);
            builder.Property(b => b.Address)
                .IsRequired()
                .HasMaxLength(255);
            builder.Property(b => b.Description)
                .IsRequired(false)
                .HasMaxLength(1000);
            builder.Property(b => b.ImageUrl)
                .IsRequired(false)
                .HasMaxLength(int.MaxValue);
            builder.HasOne(b => b.User)
                .WithOne()
                .HasForeignKey<Business>(b => b.UserId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(b => b.BusinessType)
                .WithMany(bt => bt.Businesses)
                .HasForeignKey(b => b.BusinessTypeId)
                .IsRequired().OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class PackageConfiguration : IEntityTypeConfiguration<Package>
    {
        public void Configure(EntityTypeBuilder<Package> builder)
        {
            builder.ToTable("Package");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(255);
            builder.Property(p => p.Description)
                .IsRequired(false)
                .HasMaxLength(1000);
            builder.Property(p => p.ImageUrl)
                .IsRequired(false)
                .HasMaxLength(int.MaxValue);
            builder.Property(p => p.Price)
                .HasPrecision(18, 2)
                .IsRequired();
            builder.Property(p => p.Quantity)
                .IsRequired();
            builder.Property(p => p.PickupStart)
                .IsRequired();
            builder.Property(p => p.PickupEnd)
                .IsRequired();
            builder.HasOne(p => p.Business)
                .WithMany(b => b.Packages)
                .HasForeignKey(p => p.BusinessId)
                .IsRequired().OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.PackageType)
                .WithMany(pt => pt.Packages)
                .HasForeignKey(p => p.PackageTypeId)
                .IsRequired().OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Order");
            builder.HasKey(o => o.Id);
            builder.Property(o => o.OrderNumber)
                .IsRequired();
            builder.HasOne(o => o.Business)
                .WithMany(b => b.Orders)
                .HasForeignKey(o => o.BusinessId)
                .IsRequired().OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(o => o.Status)
                .WithMany(s => s.Orders)
                .HasForeignKey(o => o.StatusId)
                .IsRequired().OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .IsRequired().OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class OrderPackageConfiguration : IEntityTypeConfiguration<OrderPackage>
    {
        public void Configure(EntityTypeBuilder<OrderPackage> builder)
        {
            builder.ToTable("OrderPackage");
            builder.HasKey(op => new { op.OrderId, op.PackageId });
            builder.Property(op => op.Quantity)
                .IsRequired();
            builder.HasOne(op => op.Order)
                .WithMany(o => o.OrderPackages)
                .HasForeignKey(op => op.OrderId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(op => op.Package)
                .WithMany(p => p.OrderPackages)
                .HasForeignKey(op => op.PackageId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}