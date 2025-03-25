using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PaymentTransaction_paymentId",
                table: "PaymentTransaction");

            migrationBuilder.AddColumn<int>(
                name: "PostId1",
                table: "SEOMetadata",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PostTypeId1",
                table: "Posts",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "notes",
                table: "Payments",
                type: "nvarchar(200)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BookingId1",
                table: "Payments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BookingId2",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "bookingId",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId1",
                table: "Cars",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TripPlans",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tripId = table.Column<int>(type: "int", nullable: false),
                    regionId = table.Column<int>(type: "int", nullable: false),
                    startDate = table.Column<DateTime>(type: "datetime2(7)", nullable: false),
                    endDate = table.Column<DateTime>(type: "datetime2(7)", nullable: false),
                    duration = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    includedServices = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    stops = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    mealsPlan = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    hotelStays = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    RegionId1 = table.Column<int>(type: "int", nullable: true),
                    TripId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripPlans", x => x.id);
                    table.ForeignKey(
                        name: "FK_TripPlans_Regions_RegionId1",
                        column: x => x.RegionId1,
                        principalTable: "Regions",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_TripPlans_Regions_regionId",
                        column: x => x.regionId,
                        principalTable: "Regions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TripPlans_Trips_TripId1",
                        column: x => x.TripId1,
                        principalTable: "Trips",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_TripPlans_Trips_tripId",
                        column: x => x.tripId,
                        principalTable: "Trips",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TripPlanCars",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tripPlanId = table.Column<int>(type: "int", nullable: false),
                    carId = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<decimal>(type: "decimal(16,2)", nullable: false),
                    TripPlanId1 = table.Column<int>(type: "int", nullable: true),
                    TripPlanId2 = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripPlanCars", x => x.id);
                    table.ForeignKey(
                        name: "FK_TripPlanCars_Cars_carId",
                        column: x => x.carId,
                        principalTable: "Cars",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TripPlanCars_TripPlans_TripPlanId1",
                        column: x => x.TripPlanId1,
                        principalTable: "TripPlans",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_TripPlanCars_TripPlans_tripPlanId",
                        column: x => x.tripPlanId,
                        principalTable: "TripPlans",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    bookingType = table.Column<bool>(type: "bit", nullable: false),
                    startDate = table.Column<DateTime>(type: "datetime2(7)", nullable: false),
                    endDate = table.Column<DateTime>(type: "datetime2(7)", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    numOfPassengers = table.Column<int>(type: "int", nullable: false),
                    TripBookingBookingId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "CarBookings",
                columns: table => new
                {
                    bookingId = table.Column<int>(type: "int", nullable: false),
                    carId = table.Column<int>(type: "int", nullable: false),
                    pickUpLocation = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    dropOffLocation = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    withDriver = table.Column<bool>(type: "bit", nullable: false),
                    CarId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarBookings", x => x.bookingId);
                    table.ForeignKey(
                        name: "FK_CarBookings_Bookings_bookingId",
                        column: x => x.bookingId,
                        principalTable: "Bookings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CarBookings_Cars_CarId1",
                        column: x => x.CarId1,
                        principalTable: "Cars",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_CarBookings_Cars_carId",
                        column: x => x.carId,
                        principalTable: "Cars",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TripBookings",
                columns: table => new
                {
                    bookingId = table.Column<int>(type: "int", nullable: false),
                    tripPlanId = table.Column<int>(type: "int", nullable: false),
                    withGuide = table.Column<bool>(type: "bit", nullable: false),
                    TripPlanId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripBookings", x => x.bookingId);
                    table.ForeignKey(
                        name: "FK_TripBookings_Bookings_bookingId",
                        column: x => x.bookingId,
                        principalTable: "Bookings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TripBookings_TripPlans_TripPlanId1",
                        column: x => x.TripPlanId1,
                        principalTable: "TripPlans",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_TripBookings_TripPlans_tripPlanId",
                        column: x => x.tripPlanId,
                        principalTable: "TripPlans",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImageShots",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    path = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    type = table.Column<bool>(type: "bit", nullable: false),
                    carBookingId = table.Column<int>(type: "int", nullable: false),
                    CarBookingBookingId = table.Column<int>(type: "int", nullable: true),
                    CarBookingBookingId1 = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageShots", x => x.id);
                    table.ForeignKey(
                        name: "FK_ImageShots_CarBookings_CarBookingBookingId",
                        column: x => x.CarBookingBookingId,
                        principalTable: "CarBookings",
                        principalColumn: "bookingId");
                    table.ForeignKey(
                        name: "FK_ImageShots_CarBookings_carBookingId",
                        column: x => x.carBookingId,
                        principalTable: "CarBookings",
                        principalColumn: "bookingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SEOMetadata_PostId1",
                table: "SEOMetadata",
                column: "PostId1");

            migrationBuilder.CreateIndex(
                name: "IX_PostTypes_title",
                table: "PostTypes",
                column: "title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_PostTypeId1",
                table: "Posts",
                column: "PostTypeId1");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransaction_paymentId_paymentMethodId_transactionDate",
                table: "PaymentTransaction",
                columns: new[] { "paymentId", "paymentMethodId", "transactionDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_bookingId",
                table: "Payments",
                column: "bookingId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_BookingId1",
                table: "Payments",
                column: "BookingId1");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethods_method",
                table: "PaymentMethods",
                column: "method",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cars_CategoryId1",
                table: "Cars",
                column: "CategoryId1");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_TripBookingBookingId",
                table: "Bookings",
                column: "TripBookingBookingId");

            migrationBuilder.CreateIndex(
                name: "IX_CarBookings_carId",
                table: "CarBookings",
                column: "carId");

            migrationBuilder.CreateIndex(
                name: "IX_CarBookings_CarId1",
                table: "CarBookings",
                column: "CarId1");

            migrationBuilder.CreateIndex(
                name: "IX_ImageShots_CarBookingBookingId",
                table: "ImageShots",
                column: "CarBookingBookingId");

            migrationBuilder.CreateIndex(
                name: "IX_ImageShots_carBookingId",
                table: "ImageShots",
                column: "carBookingId");

            migrationBuilder.CreateIndex(
                name: "IX_TripBookings_tripPlanId",
                table: "TripBookings",
                column: "tripPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_TripBookings_TripPlanId1",
                table: "TripBookings",
                column: "TripPlanId1");

            migrationBuilder.CreateIndex(
                name: "IX_TripPlanCars_carId",
                table: "TripPlanCars",
                column: "carId");

            migrationBuilder.CreateIndex(
                name: "IX_TripPlanCars_tripPlanId",
                table: "TripPlanCars",
                column: "tripPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_TripPlanCars_TripPlanId1",
                table: "TripPlanCars",
                column: "TripPlanId1");

            migrationBuilder.CreateIndex(
                name: "IX_TripPlans_regionId",
                table: "TripPlans",
                column: "regionId");

            migrationBuilder.CreateIndex(
                name: "IX_TripPlans_RegionId1",
                table: "TripPlans",
                column: "RegionId1");

            migrationBuilder.CreateIndex(
                name: "IX_TripPlans_tripId",
                table: "TripPlans",
                column: "tripId");

            migrationBuilder.CreateIndex(
                name: "IX_TripPlans_TripId1",
                table: "TripPlans",
                column: "TripId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_Categories_CategoryId1",
                table: "Cars",
                column: "CategoryId1",
                principalTable: "Categories",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Bookings_BookingId1",
                table: "Payments",
                column: "BookingId1",
                principalTable: "Bookings",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Bookings_bookingId",
                table: "Payments",
                column: "bookingId",
                principalTable: "Bookings",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_PostTypes_PostTypeId1",
                table: "Posts",
                column: "PostTypeId1",
                principalTable: "PostTypes",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_SEOMetadata_Posts_PostId1",
                table: "SEOMetadata",
                column: "PostId1",
                principalTable: "Posts",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_TripBookings_TripBookingBookingId",
                table: "Bookings",
                column: "TripBookingBookingId",
                principalTable: "TripBookings",
                principalColumn: "bookingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_Categories_CategoryId1",
                table: "Cars");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Bookings_BookingId1",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Bookings_bookingId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_PostTypes_PostTypeId1",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_SEOMetadata_Posts_PostId1",
                table: "SEOMetadata");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_TripBookings_TripBookingBookingId",
                table: "Bookings");

            migrationBuilder.DropTable(
                name: "ImageShots");

            migrationBuilder.DropTable(
                name: "TripPlanCars");

            migrationBuilder.DropTable(
                name: "CarBookings");

            migrationBuilder.DropTable(
                name: "TripBookings");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "TripPlans");

            migrationBuilder.DropIndex(
                name: "IX_SEOMetadata_PostId1",
                table: "SEOMetadata");

            migrationBuilder.DropIndex(
                name: "IX_PostTypes_title",
                table: "PostTypes");

            migrationBuilder.DropIndex(
                name: "IX_Posts_PostTypeId1",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_PaymentTransaction_paymentId_paymentMethodId_transactionDate",
                table: "PaymentTransaction");

            migrationBuilder.DropIndex(
                name: "IX_Payments_bookingId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_BookingId1",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_PaymentMethods_method",
                table: "PaymentMethods");

            migrationBuilder.DropIndex(
                name: "IX_Cars_CategoryId1",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "PostId1",
                table: "SEOMetadata");

            migrationBuilder.DropColumn(
                name: "PostTypeId1",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "BookingId1",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "BookingId2",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "bookingId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "CategoryId1",
                table: "Cars");

            migrationBuilder.AlterColumn<string>(
                name: "notes",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransaction_paymentId",
                table: "PaymentTransaction",
                column: "paymentId");
        }
    }
}
