using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EcoMeal.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Package",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.InsertData(
                table: "BusinessType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "Restaurant" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "Supermarket" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), "Bakery" }
                });

            migrationBuilder.InsertData(
                table: "PackageType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("44444444-4444-4444-4444-444444444444"), "Surprise Bag" },
                    { new Guid("55555555-5555-5555-5555-555555555555"), "Pastry Bag" },
                    { new Guid("9cab424e-fb07-4777-ac7b-c8d72bf2f803"), "Vegan Bag" }
                });

            migrationBuilder.InsertData(
                table: "Business",
                columns: new[] { "Id", "Address", "BusinessTypeId", "Description", "ImageUrl", "Name" },
                values: new object[,]
                {
                    { new Guid("66666666-6666-6666-6666-666666666666"), "Str. Victoriei, Nr. 10", new Guid("11111111-1111-1111-1111-111111111111"), "A cozy place with sustainable and delicious food.", "https://images.unsplash.com/photo-1517248135467-4c7edcad34c4", "Green Bite Bistro" },
                    { new Guid("77777777-7777-7777-7777-777777777777"), "Str. Libertății, Nr. 5", new Guid("22222222-2222-2222-2222-222222222222"), "Your local supermarket for fresh produce and essentials.", "https://images.unsplash.com/photo-1586201375761-83865001e3b6", "FreshMart" }
                });

            migrationBuilder.InsertData(
                table: "Package",
                columns: new[] { "Id", "BusinessId", "Description", "ImageUrl", "Name", "PackageTypeId", "PickupEnd", "PickupStart", "Price", "Quantity" },
                values: new object[,]
                {
                    { new Guid("5abcdcf0-d006-4222-86af-d0b15c55c0cd"), new Guid("77777777-7777-7777-7777-777777777777"), "Perfect for toast, sandwiches or making breadcrumbs.", "https://images.unsplash.com/photo-1509440159596-0249088772ff", "Yesterday's Bread Bundle", new Guid("55555555-5555-5555-5555-555555555555"), new DateTime(2026, 7, 7, 20, 0, 0, 0, DateTimeKind.Local), new DateTime(2026, 7, 7, 16, 0, 0, 0, DateTimeKind.Local), 3.5m, 20 },
                    { new Guid("e377abc4-1106-4802-a945-8f5b275c3316"), new Guid("66666666-6666-6666-6666-666666666666"), "Delicious leftover meals perfectly fine to eat.", "https://images.unsplash.com/photo-1542838132-92c53300491e", "End of Day Surprise", new Guid("44444444-4444-4444-4444-444444444444"), new DateTime(2026, 7, 7, 20, 0, 0, 0, DateTimeKind.Local), new DateTime(2026, 7, 7, 18, 0, 0, 0, DateTimeKind.Local), 15.5m, 5 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BusinessType",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "Package",
                keyColumn: "Id",
                keyValue: new Guid("5abcdcf0-d006-4222-86af-d0b15c55c0cd"));

            migrationBuilder.DeleteData(
                table: "Package",
                keyColumn: "Id",
                keyValue: new Guid("e377abc4-1106-4802-a945-8f5b275c3316"));

            migrationBuilder.DeleteData(
                table: "PackageType",
                keyColumn: "Id",
                keyValue: new Guid("9cab424e-fb07-4777-ac7b-c8d72bf2f803"));

            migrationBuilder.DeleteData(
                table: "Business",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"));

            migrationBuilder.DeleteData(
                table: "Business",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"));

            migrationBuilder.DeleteData(
                table: "PackageType",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"));

            migrationBuilder.DeleteData(
                table: "PackageType",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"));

            migrationBuilder.DeleteData(
                table: "BusinessType",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "BusinessType",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.AlterColumn<float>(
                name: "Price",
                table: "Package",
                type: "real",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2);
        }
    }
}
