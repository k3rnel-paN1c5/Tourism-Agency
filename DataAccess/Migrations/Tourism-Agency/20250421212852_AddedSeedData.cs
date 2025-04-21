using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.Migrations.TourismAgency
{
    /// <inheritdoc />
    public partial class AddedSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "id", "title" },
                values: new object[,]
                {
                    { 1, "Sedan" },
                    { 2, "SUV" },
                    { 3, "Sports" }
                });

            migrationBuilder.InsertData(
                table: "Cars",
                columns: new[] { "id", "categoryId", "color", "image", "mbw", "model", "ppd", "pph", "seats" },
                values: new object[,]
                {
                    { 1, 1, "Silver", "toyota-camry.jpg", 1.00m, "Toyota Camry", 80.00m, 15.00m, 5 },
                    { 2, 2, "White", "honda-cr-v.jpg", 2.00m, "Honda CR-V", 100.00m, 20.00m, 7 },
                    { 3, 3, "Red", "ford-mustang.jpg", 3.00m, "Ford Mustang", 150.00m, 30.00m, 4 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "id",
                keyValue: 3);
        }
    }
}
