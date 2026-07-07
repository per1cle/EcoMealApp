using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EcoMeal.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataValues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.InsertData(
                table: "Package",
                columns: new[] { "Id", "BusinessId", "Description", "ImageUrl", "Name", "PackageTypeId", "PickupEnd", "PickupStart", "Price", "Quantity" },
                values: new object[,]
                {
                    { new Guid("88888888-8888-8888-8888-888888888888"), new Guid("66666666-6666-6666-6666-666666666666"), "Delicious leftover meals perfectly fine to eat.", "https://images.unsplash.com/photo-1542838132-92c53300491e", "End of Day Surprise", new Guid("44444444-4444-4444-4444-444444444444"), new DateTime(2026, 7, 8, 22, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 7, 8, 18, 0, 0, 0, DateTimeKind.Unspecified), 15.5m, 5 },
                    { new Guid("99999999-9999-9999-9999-999999999999"), new Guid("77777777-7777-7777-7777-777777777777"), "Perfect for toast, sandwiches or making breadcrumbs.", "https://images.unsplash.com/photo-1509440159596-0249088772ff", "Yesterday's Bread Bundle", new Guid("55555555-5555-5555-5555-555555555555"), new DateTime(2026, 7, 8, 20, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 7, 8, 16, 0, 0, 0, DateTimeKind.Unspecified), 3.5m, 20 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Package",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"));

            migrationBuilder.DeleteData(
                table: "Package",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"));

            migrationBuilder.InsertData(
                table: "Package",
                columns: new[] { "Id", "BusinessId", "Description", "ImageUrl", "Name", "PackageTypeId", "PickupEnd", "PickupStart", "Price", "Quantity" },
                values: new object[,]
                {
                    { new Guid("5abcdcf0-d006-4222-86af-d0b15c55c0cd"), new Guid("77777777-7777-7777-7777-777777777777"), "Perfect for toast, sandwiches or making breadcrumbs.", "https://images.unsplash.com/photo-1509440159596-0249088772ff", "Yesterday's Bread Bundle", new Guid("55555555-5555-5555-5555-555555555555"), new DateTime(2026, 7, 7, 20, 0, 0, 0, DateTimeKind.Local), new DateTime(2026, 7, 7, 16, 0, 0, 0, DateTimeKind.Local), 3.5m, 20 },
                    { new Guid("e377abc4-1106-4802-a945-8f5b275c3316"), new Guid("66666666-6666-6666-6666-666666666666"), "Delicious leftover meals perfectly fine to eat.", "https://images.unsplash.com/photo-1542838132-92c53300491e", "End of Day Surprise", new Guid("44444444-4444-4444-4444-444444444444"), new DateTime(2026, 7, 7, 20, 0, 0, 0, DateTimeKind.Local), new DateTime(2026, 7, 7, 18, 0, 0, 0, DateTimeKind.Local), 15.5m, 5 }
                });

            migrationBuilder.InsertData(
                table: "PackageType",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("9cab424e-fb07-4777-ac7b-c8d72bf2f803"), "Vegan Bag" });
        }
    }
}
