using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarBrand.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCarBrandSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CarBrand",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "BrandName", "CountryOrigin", "Year" },
                values: new object[] { "Audi", "Germany", 1932 });

            migrationBuilder.UpdateData(
                table: "CarBrand",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "BrandName", "Year" },
                values: new object[] { "Chevrolet", 1911 });

            migrationBuilder.UpdateData(
                table: "CarBrand",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "BrandName", "Year" },
                values: new object[] { "Porche", 1931 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "CarBrand",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "BrandName", "CountryOrigin", "Year" },
                values: new object[] { "Toyota", "Japan", 1937 });

            migrationBuilder.UpdateData(
                table: "CarBrand",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "BrandName", "Year" },
                values: new object[] { "Ford", 1903 });

            migrationBuilder.UpdateData(
                table: "CarBrand",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "BrandName", "Year" },
                values: new object[] { "Volkswagen", 1937 });
        }
    }
}
