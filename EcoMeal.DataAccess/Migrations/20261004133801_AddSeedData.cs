using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EcoMeal.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000001"), "10000000-0000-0000-0000-000000000001", "Admin", "ADMIN" },
                    { new Guid("10000000-0000-0000-0000-000000000002"), "10000000-0000-0000-0000-000000000002", "Business", "BUSINESS" },
                    { new Guid("10000000-0000-0000-0000-000000000003"), "10000000-0000-0000-0000-000000000003", "Customer", "CUSTOMER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), 0, "a7251786-8a7e-4043-9ce6-15f1f9e20a32", "bistro@ecomeal.com", true, false, null, "Green Bite Bistro", "BISTRO@ECOMEAL.COM", "BISTRO@ECOMEAL.COM", "AQAAAAIAAYagAAAAEK6QRy078UZfLCymeFSi8u2/PAgSuWVrg9NHcr/NkHITgrJhS6S4kwlc22j0lVdXIg==", null, false, "d87a55be-2bf3-4614-bfe5-dcf46e5b4ff4", false, "bistro@ecomeal.com" },
                    { new Guid("00000000-0000-0000-0000-000000000002"), 0, "b9bf8b58-868d-4f1b-b463-547781b16866", "freshmart@ecomeal.com", true, false, null, "FreshMart", "FRESHMART@ECOMEAL.COM", "FRESHMART@ECOMEAL.COM", "AQAAAAIAAYagAAAAEK6QRy078UZfLCymeFSi8u2/PAgSuWVrg9NHcr/NkHITgrJhS6S4kwlc22j0lVdXIg==", null, false, "f0896082-9626-4448-96ef-23e5a5a1f6a1", false, "freshmart@ecomeal.com" },
                    { new Guid("00000000-0000-0000-0000-000000000003"), 0, "c5b2e04e-28b7-4db5-b384-95d43b2f5d91", "admin@ecomeal.com", true, false, null, "Admin", "ADMIN@ECOMEAL.COM", "ADMIN@ECOMEAL.COM", "AQAAAAIAAYagAAAAEK6QRy078UZfLCymeFSi8u2/PAgSuWVrg9NHcr/NkHITgrJhS6S4kwlc22j0lVdXIg==", null, false, "e98e4f50-3a33-470b-8d07-ee65f6c88f11", false, "admin@ecomeal.com" }
                });

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
                    { new Guid("55555555-5555-5555-5555-555555555555"), "Pastry Bag" }
                });

            migrationBuilder.InsertData(
                table: "Status",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("224f9b84-e878-4b04-bfb9-6bc6b3eba8dd"), "Cancelled" },
                    { new Guid("24feb601-b7fa-4e23-aa03-f121fad19347"), "Completed" },
                    { new Guid("45b77eca-efe4-4a2e-867d-7eb6170d3703"), "Confirmed" },
                    { new Guid("e2712cd8-cd80-4f27-9711-c676f84e339c"), "Pending" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000002"), new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("10000000-0000-0000-0000-000000000002"), new Guid("00000000-0000-0000-0000-000000000002") },
                    { new Guid("10000000-0000-0000-0000-000000000001"), new Guid("00000000-0000-0000-0000-000000000003") }
                });

            migrationBuilder.InsertData(
                table: "Business",
                columns: new[] { "Id", "Address", "BusinessTypeId", "Description", "ImageUrl", "Name", "UserId" },
                values: new object[,]
                {
                    { new Guid("66666666-6666-6666-6666-666666666666"), "Str. Victoriei, Nr. 10", new Guid("11111111-1111-1111-1111-111111111111"), "A cozy place with sustainable and delicious food.", "https://images.unsplash.com/photo-1517248135467-4c7edcad34c4", "Green Bite Bistro", new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("77777777-7777-7777-7777-777777777777"), "Str. Libertății, Nr. 5", new Guid("22222222-2222-2222-2222-222222222222"), "Your local supermarket for fresh produce and essentials.", "https://images.unsplash.com/photo-1586201375761-83865001e3b6", "FreshMart", new Guid("00000000-0000-0000-0000-000000000002") }
                });

            migrationBuilder.InsertData(
                table: "Package",
                columns: new[] { "Id", "BusinessId", "Description", "ImageUrl", "Name", "PackageTypeId", "PickupEnd", "PickupStart", "Price", "Quantity" },
                values: new object[,]
                {
                    { new Guid("88888888-8888-8888-8888-888888888888"), new Guid("66666666-6666-6666-6666-666666666666"), "Delicious leftover meals perfectly fine to eat.", "https://images.unsplash.com/photo-1542838132-92c53300491e", "End of Day Surprise", new Guid("44444444-4444-4444-4444-444444444444"), new DateTime(2026, 7, 8, 22, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 7, 8, 18, 0, 0, 0, DateTimeKind.Unspecified), 15.50m, 5 },
                    { new Guid("99999999-9999-9999-9999-999999999999"), new Guid("77777777-7777-7777-7777-777777777777"), "Perfect for toast, sandwiches or making breadcrumbs.", "https://images.unsplash.com/photo-1509440159596-0249088772ff", "Yesterday's Bread Bundle", new Guid("55555555-5555-5555-5555-555555555555"), new DateTime(2026, 7, 8, 20, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 7, 8, 16, 0, 0, 0, DateTimeKind.Unspecified), 3.50m, 20 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("10000000-0000-0000-0000-000000000002"), new Guid("00000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("10000000-0000-0000-0000-000000000002"), new Guid("00000000-0000-0000-0000-000000000002") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("10000000-0000-0000-0000-000000000001"), new Guid("00000000-0000-0000-0000-000000000003") });

            migrationBuilder.DeleteData(
                table: "BusinessType",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "Package",
                keyColumn: "Id",
                keyValue: new Guid("88888888-8888-8888-8888-888888888888"));

            migrationBuilder.DeleteData(
                table: "Package",
                keyColumn: "Id",
                keyValue: new Guid("99999999-9999-9999-9999-999999999999"));

            migrationBuilder.DeleteData(
                table: "Status",
                keyColumn: "Id",
                keyValue: new Guid("224f9b84-e878-4b04-bfb9-6bc6b3eba8dd"));

            migrationBuilder.DeleteData(
                table: "Status",
                keyColumn: "Id",
                keyValue: new Guid("24feb601-b7fa-4e23-aa03-f121fad19347"));

            migrationBuilder.DeleteData(
                table: "Status",
                keyColumn: "Id",
                keyValue: new Guid("45b77eca-efe4-4a2e-867d-7eb6170d3703"));

            migrationBuilder.DeleteData(
                table: "Status",
                keyColumn: "Id",
                keyValue: new Guid("e2712cd8-cd80-4f27-9711-c676f84e339c"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"));

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
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "BusinessType",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "BusinessType",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));
        }
    }
}
