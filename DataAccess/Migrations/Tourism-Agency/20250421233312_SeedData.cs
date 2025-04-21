using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.Migrations.TourismAgency
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "id", "hireDate" },
                values: new object[,]
                {
                    { "emp1", new DateTime(2020, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { "emp2", new DateTime(2018, 8, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { "emp3", new DateTime(2022, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "id",
                keyValue: "emp1");

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "id",
                keyValue: "emp2");

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "id",
                keyValue: "emp3");
        }
    }
}
