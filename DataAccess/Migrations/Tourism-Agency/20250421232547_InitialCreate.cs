using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.Migrations.TourismAgency
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    firstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    lastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    phoneNumber = table.Column<string>(type: "char(12)", nullable: false),
                    whatsapp = table.Column<string>(type: "char(14)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    hireDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethods",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    method = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    icon = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethods", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "PostTypes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(200)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostTypes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Trips",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    slug = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    isAvailable = table.Column<bool>(type: "bit", nullable: false),
                    description = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    isPrivate = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trips", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    model = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    seats = table.Column<int>(type: "int", nullable: false),
                    color = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    image = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    pph = table.Column<decimal>(type: "decimal(16,2)", nullable: false),
                    ppd = table.Column<decimal>(type: "decimal(16,2)", nullable: false),
                    mbw = table.Column<decimal>(type: "decimal(16,2)", nullable: false),
                    categoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.id);
                    table.ForeignKey(
                        name: "FK_Cars_Categories_categoryId",
                        column: x => x.categoryId,
                        principalTable: "Categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
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
                    status = table.Column<string>(type: "nvarchar(30)", nullable: false),
                    numOfPassengers = table.Column<int>(type: "int", nullable: false),
                    customerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    employeeId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.id);
                    table.ForeignKey(
                        name: "FK_Bookings_Customers_customerId",
                        column: x => x.customerId,
                        principalTable: "Customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bookings_Employees_employeeId",
                        column: x => x.employeeId,
                        principalTable: "Employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    body = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    image = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    slug = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    views = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<string>(type: "nvarchar(30)", nullable: false),
                    summary = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    publishDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    postTypeId = table.Column<int>(type: "int", nullable: false),
                    employeeId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.id);
                    table.ForeignKey(
                        name: "FK_Posts_Employees_employeeId",
                        column: x => x.employeeId,
                        principalTable: "Employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Posts_PostTypes_postTypeId",
                        column: x => x.postTypeId,
                        principalTable: "PostTypes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

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
                    duration = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    includedServices = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    stops = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    mealsPlan = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    hotelStays = table.Column<string>(type: "nvarchar(200)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripPlans", x => x.id);
                    table.ForeignKey(
                        name: "FK_TripPlans_Regions_regionId",
                        column: x => x.regionId,
                        principalTable: "Regions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TripPlans_Trips_tripId",
                        column: x => x.tripId,
                        principalTable: "Trips",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CarBookings",
                columns: table => new
                {
                    bookingId = table.Column<int>(type: "int", nullable: false),
                    carId = table.Column<int>(type: "int", nullable: false),
                    pickUpLocation = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    dropOffLocation = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    withDriver = table.Column<bool>(type: "bit", nullable: false)
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
                        name: "FK_CarBookings_Cars_carId",
                        column: x => x.carId,
                        principalTable: "Cars",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    bookingId = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<string>(type: "nvarchar(30)", nullable: false),
                    amountDue = table.Column<decimal>(type: "decimal(16,2)", nullable: false),
                    amountPaid = table.Column<decimal>(type: "decimal(16,2)", nullable: false),
                    paymentDate = table.Column<DateTime>(type: "datetime2(7)", nullable: false),
                    notes = table.Column<string>(type: "nvarchar(200)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.id);
                    table.ForeignKey(
                        name: "FK_Payments_Bookings_bookingId",
                        column: x => x.bookingId,
                        principalTable: "Bookings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostTags",
                columns: table => new
                {
                    PostId = table.Column<int>(type: "int", nullable: false),
                    TagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostTags", x => new { x.PostId, x.TagId });
                    table.ForeignKey(
                        name: "FK_PostTags_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SEOMetadata",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    urlSlug = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    metaTitle = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    metaDescription = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    metaKeywords = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    postId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SEOMetadata", x => x.id);
                    table.ForeignKey(
                        name: "FK_SEOMetadata_Posts_postId",
                        column: x => x.postId,
                        principalTable: "Posts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TripBookings",
                columns: table => new
                {
                    bookingId = table.Column<int>(type: "int", nullable: false),
                    tripPlanId = table.Column<int>(type: "int", nullable: false),
                    withGuide = table.Column<bool>(type: "bit", nullable: false)
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
                        name: "FK_TripBookings_TripPlans_tripPlanId",
                        column: x => x.tripPlanId,
                        principalTable: "TripPlans",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TripPlanCars",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tripPlanId = table.Column<int>(type: "int", nullable: false),
                    carId = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<decimal>(type: "decimal(16,2)", nullable: false)
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
                        name: "FK_TripPlanCars_TripPlans_tripPlanId",
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
                    carBookingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageShots", x => x.id);
                    table.ForeignKey(
                        name: "FK_ImageShots_CarBookings_carBookingId",
                        column: x => x.carBookingId,
                        principalTable: "CarBookings",
                        principalColumn: "bookingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentTransaction",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    type = table.Column<string>(type: "nvarchar(30)", nullable: false),
                    amount = table.Column<decimal>(type: "decimal(16,2)", nullable: false),
                    transactionDate = table.Column<DateTime>(type: "datetime2(7)", nullable: false),
                    paymentId = table.Column<int>(type: "int", nullable: false),
                    paymentMethodId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentTransaction", x => x.id);
                    table.ForeignKey(
                        name: "FK_PaymentTransaction_PaymentMethods_paymentMethodId",
                        column: x => x.paymentMethodId,
                        principalTable: "PaymentMethods",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentTransaction_Payments_paymentId",
                        column: x => x.paymentId,
                        principalTable: "Payments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                table: "Customers",
                columns: new[] { "id", "Country", "firstName", "lastName", "phoneNumber", "whatsapp" },
                values: new object[,]
                {
                    { "user1", "USA", "John", "Doe", "+1234567890", "+1234567890" },
                    { "user2", "Canada", "Jane", "Smith", "+0987654321", "+0987654321" },
                    { "user3", "UK", "Alice", "Johnson", "+1122334455", "+1122334455" }
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

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_customerId",
                table: "Bookings",
                column: "customerId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_employeeId",
                table: "Bookings",
                column: "employeeId");

            migrationBuilder.CreateIndex(
                name: "IX_CarBookings_carId",
                table: "CarBookings",
                column: "carId");

            migrationBuilder.CreateIndex(
                name: "IX_Cars_categoryId",
                table: "Cars",
                column: "categoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_title",
                table: "Categories",
                column: "title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ImageShots_carBookingId",
                table: "ImageShots",
                column: "carBookingId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethods_method",
                table: "PaymentMethods",
                column: "method",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_bookingId",
                table: "Payments",
                column: "bookingId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransaction_paymentId_paymentMethodId_transactionDate",
                table: "PaymentTransaction",
                columns: new[] { "paymentId", "paymentMethodId", "transactionDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransaction_paymentMethodId",
                table: "PaymentTransaction",
                column: "paymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_employeeId",
                table: "Posts",
                column: "employeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_postTypeId",
                table: "Posts",
                column: "postTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PostTags_TagId",
                table: "PostTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_PostTypes_title",
                table: "PostTypes",
                column: "title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Regions_name",
                table: "Regions",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SEOMetadata_postId",
                table: "SEOMetadata",
                column: "postId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_name",
                table: "Tags",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TripBookings_tripPlanId",
                table: "TripBookings",
                column: "tripPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_TripPlanCars_carId",
                table: "TripPlanCars",
                column: "carId");

            migrationBuilder.CreateIndex(
                name: "IX_TripPlanCars_tripPlanId",
                table: "TripPlanCars",
                column: "tripPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_TripPlans_regionId",
                table: "TripPlans",
                column: "regionId");

            migrationBuilder.CreateIndex(
                name: "IX_TripPlans_tripId",
                table: "TripPlans",
                column: "tripId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_name",
                table: "Trips",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trips_slug",
                table: "Trips",
                column: "slug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImageShots");

            migrationBuilder.DropTable(
                name: "PaymentTransaction");

            migrationBuilder.DropTable(
                name: "PostTags");

            migrationBuilder.DropTable(
                name: "SEOMetadata");

            migrationBuilder.DropTable(
                name: "TripBookings");

            migrationBuilder.DropTable(
                name: "TripPlanCars");

            migrationBuilder.DropTable(
                name: "CarBookings");

            migrationBuilder.DropTable(
                name: "PaymentMethods");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "TripPlans");

            migrationBuilder.DropTable(
                name: "Cars");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "PostTypes");

            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.DropTable(
                name: "Trips");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
