using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoMeal.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddBusinessCoordinates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Business",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Business",
                type: "float",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Business",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                columns: new[] { "Latitude", "Longitude" },
                values: new object[] { 44.3185, 23.799800000000001 });

            migrationBuilder.UpdateData(
                table: "Business",
                keyColumn: "Id",
                keyValue: new Guid("77777777-7777-7777-7777-777777777777"),
                columns: new[] { "Latitude", "Longitude" },
                values: new object[] { 44.323500000000003, 23.805 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Business");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Business");
        }
    }
}
