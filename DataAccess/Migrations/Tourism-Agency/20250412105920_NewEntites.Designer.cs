﻿// <auto-generated />
using System;
using DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataAccess.Migrations.TourismAgency
{
    [DbContext(typeof(TourismAgencyDbContext))]
    [Migration("20250412105920_NewEntites")]
    partial class NewEntites
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DataAccess.Entities.Booking", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("BookingType")
                        .HasColumnType("bit")
                        .HasColumnName("bookingType");

                    b.Property<string>("CustomerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("customerId");

                    b.Property<string>("EmployeeId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("employeeId");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2(7)")
                        .HasColumnName("endDate");

                    b.Property<int>("NumOfPassengers")
                        .HasColumnType("int")
                        .HasColumnName("numOfPassengers");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2(7)")
                        .HasColumnName("startDate");

                    b.Property<int>("Status")
                        .HasColumnType("int")
                        .HasColumnName("status");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("Bookings", (string)null);
                });

            modelBuilder.Entity("DataAccess.Entities.Car", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("int")
                        .HasColumnName("categoryId");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("color");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("image");

                    b.Property<decimal>("Mbw")
                        .HasColumnType("decimal(16,2)")
                        .HasColumnName("mbw");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("model");

                    b.Property<decimal>("Ppd")
                        .HasColumnType("decimal(16,2)")
                        .HasColumnName("ppd");

                    b.Property<decimal>("Pph")
                        .HasColumnType("decimal(16,2)")
                        .HasColumnName("pph");

                    b.Property<int>("Seats")
                        .HasColumnType("int")
                        .HasColumnName("seats");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Cars", (string)null);
                });

            modelBuilder.Entity("DataAccess.Entities.CarBooking", b =>
                {
                    b.Property<int>("BookingId")
                        .HasColumnType("int")
                        .HasColumnName("bookingId");

                    b.Property<int>("CarId")
                        .HasColumnType("int")
                        .HasColumnName("carId");

                    b.Property<string>("DropOffLocation")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("dropOffLocation");

                    b.Property<string>("PickUpLocation")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("pickUpLocation");

                    b.Property<int>("Status")
                        .HasColumnType("int")
                        .HasColumnName("status");

                    b.Property<bool>("WithDriver")
                        .HasColumnType("bit")
                        .HasColumnName("withDriver");

                    b.HasKey("BookingId");

                    b.HasIndex("CarId");

                    b.ToTable("CarBookings", (string)null);
                });

            modelBuilder.Entity("DataAccess.Entities.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("title");

                    b.HasKey("Id");

                    b.HasIndex("Title")
                        .IsUnique();

                    b.ToTable("Categories", (string)null);
                });

            modelBuilder.Entity("DataAccess.Entities.Customer", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("id");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Country");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("firstName");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("lastName");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("char(12)")
                        .HasColumnName("phoneNumber");

                    b.Property<string>("Whatsapp")
                        .HasColumnType("char(14)")
                        .HasColumnName("whatsapp");

                    b.HasKey("UserId");

                    b.ToTable("Customers", (string)null);
                });

            modelBuilder.Entity("DataAccess.Entities.Employee", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("id");

                    b.Property<DateTime>("HireDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("hireDate");

                    b.HasKey("UserId");

                    b.ToTable("Employees", (string)null);
                });

            modelBuilder.Entity("DataAccess.Entities.ImageShot", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CarBookingId")
                        .HasColumnType("int")
                        .HasColumnName("carBookingId");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("path");

                    b.Property<bool>("Type")
                        .HasColumnType("bit")
                        .HasColumnName("type");

                    b.HasKey("Id");

                    b.HasIndex("CarBookingId");

                    b.ToTable("ImageShots", (string)null);
                });

            modelBuilder.Entity("DataAccess.Entities.Payment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("AmountDue")
                        .HasColumnType("decimal(16,2)")
                        .HasColumnName("amountDue");

                    b.Property<decimal>("AmountPaid")
                        .HasColumnType("decimal(16,2)")
                        .HasColumnName("amountPaid");

                    b.Property<int>("BookingId")
                        .HasColumnType("int")
                        .HasColumnName("bookingId");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(200)")
                        .HasColumnName("notes");

                    b.Property<DateTime>("PaymentDate")
                        .HasColumnType("datetime2(7)")
                        .HasColumnName("paymentDate");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("status");

                    b.HasKey("Id");

                    b.HasIndex("BookingId");

                    b.ToTable("Payments", (string)null);
                });

            modelBuilder.Entity("DataAccess.Entities.PaymentMethod", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Icon")
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("icon");

                    b.Property<string>("Method")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("method");

                    b.HasKey("Id");

                    b.HasIndex("Method")
                        .IsUnique();

                    b.ToTable("PaymentMethods", (string)null);
                });

            modelBuilder.Entity("DataAccess.Entities.PaymentTransaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(16,2)")
                        .HasColumnName("amount");

                    b.Property<int>("PaymentId")
                        .HasColumnType("int")
                        .HasColumnName("paymentId");

                    b.Property<int>("PaymentMethodId")
                        .HasColumnType("int")
                        .HasColumnName("paymentMethodId");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("datetime2(7)")
                        .HasColumnName("transactionDate");

                    b.Property<string>("TransactionType")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)")
                        .HasColumnName("type");

                    b.HasKey("Id");

                    b.HasIndex("PaymentMethodId");

                    b.HasIndex("PaymentId", "PaymentMethodId", "TransactionDate")
                        .IsUnique();

                    b.ToTable("PaymentTransaction", (string)null);
                });

            modelBuilder.Entity("DataAccess.Entities.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("nvarchar(500)")
                        .HasColumnName("body");

                    b.Property<string>("EmployeeId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("employeeId");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("image");

                    b.Property<int>("PostTypeId")
                        .HasColumnType("int")
                        .HasColumnName("postTypeId");

                    b.Property<DateTime>("PublishDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("publishDate");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("slug");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("status");

                    b.Property<string>("Summary")
                        .HasColumnType("nvarchar(200)")
                        .HasColumnName("summary");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("title");

                    b.Property<int>("Views")
                        .HasColumnType("int")
                        .HasColumnName("views");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("PostTypeId");

                    b.ToTable("Posts", (string)null);
                });

            modelBuilder.Entity("DataAccess.Entities.PostTag", b =>
                {
                    b.Property<int>("PostId")
                        .HasColumnType("int");

                    b.Property<int>("TagId")
                        .HasColumnType("int");

                    b.HasKey("PostId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("PostTags", (string)null);
                });

            modelBuilder.Entity("DataAccess.Entities.PostType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(200)")
                        .HasColumnName("description");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("title");

                    b.HasKey("Id");

                    b.HasIndex("Title")
                        .IsUnique();

                    b.ToTable("PostTypes", (string)null);
                });

            modelBuilder.Entity("DataAccess.Entities.Region", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Regions", (string)null);
                });

            modelBuilder.Entity("DataAccess.Entities.SEOMetadata", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("MetaDescription")
                        .HasColumnType("nvarchar(200)")
                        .HasColumnName("metaDescription");

                    b.Property<string>("MetaKeywords")
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("metaKeywords");

                    b.Property<string>("MetaTitle")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("metaTitle");

                    b.Property<int>("PostId")
                        .HasColumnType("int")
                        .HasColumnName("postId");

                    b.Property<string>("UrlSlug")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("urlSlug");

                    b.HasKey("Id");

                    b.HasIndex("PostId");

                    b.ToTable("SEOMetadata", (string)null);
                });

            modelBuilder.Entity("DataAccess.Entities.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Tags", (string)null);
                });

            modelBuilder.Entity("DataAccess.Entities.Trip", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(200)")
                        .HasColumnName("description");

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("bit")
                        .HasColumnName("isAvailable");

                    b.Property<bool>("IsPrivate")
                        .HasColumnType("bit")
                        .HasColumnName("isPrivate");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("name");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("slug");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("Slug")
                        .IsUnique();

                    b.ToTable("Trips", (string)null);
                });

            modelBuilder.Entity("DataAccess.Entities.TripBooking", b =>
                {
                    b.Property<int>("BookingId")
                        .HasColumnType("int")
                        .HasColumnName("bookingId");

                    b.Property<int>("TripPlanId")
                        .HasColumnType("int")
                        .HasColumnName("tripPlanId");

                    b.Property<bool>("WithGuide")
                        .HasColumnType("bit")
                        .HasColumnName("withGuide");

                    b.HasKey("BookingId");

                    b.HasIndex("TripPlanId");

                    b.ToTable("TripBookings", (string)null);
                });

            modelBuilder.Entity("DataAccess.Entities.TripPlan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Duration")
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("duration");

                    b.Property<DateTime>("EndtDate")
                        .HasColumnType("datetime2(7)")
                        .HasColumnName("endDate");

                    b.Property<string>("HotelStays")
                        .HasColumnType("nvarchar(200)")
                        .HasColumnName("hotelStays");

                    b.Property<string>("IncludedServices")
                        .HasColumnType("nvarchar(200)")
                        .HasColumnName("includedServices");

                    b.Property<string>("MealsPlan")
                        .HasColumnType("nvarchar(200)")
                        .HasColumnName("mealsPlan");

                    b.Property<int>("RegionId")
                        .HasColumnType("int")
                        .HasColumnName("regionId");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2(7)")
                        .HasColumnName("startDate");

                    b.Property<string>("Stops")
                        .HasColumnType("nvarchar(200)")
                        .HasColumnName("stops");

                    b.Property<int>("TripId")
                        .HasColumnType("int")
                        .HasColumnName("tripId");

                    b.HasKey("Id");

                    b.HasIndex("RegionId");

                    b.HasIndex("TripId");

                    b.ToTable("TripPlans", (string)null);
                });

            modelBuilder.Entity("DataAccess.Entities.TripPlanCar", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CarId")
                        .HasColumnType("int")
                        .HasColumnName("carId");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(16,2)")
                        .HasColumnName("price");

                    b.Property<int>("TripPlanId")
                        .HasColumnType("int")
                        .HasColumnName("tripPlanId");

                    b.HasKey("Id");

                    b.HasIndex("CarId");

                    b.HasIndex("TripPlanId");

                    b.ToTable("TripPlanCars", (string)null);
                });

            modelBuilder.Entity("DataAccess.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("DataAccess.Entities.Booking", b =>
                {
                    b.HasOne("DataAccess.Entities.Customer", "Customer")
                        .WithMany("Bookings")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("DataAccess.Entities.Employee", "Employee")
                        .WithMany("Bookings")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("DataAccess.Entities.Car", b =>
                {
                    b.HasOne("DataAccess.Entities.Category", "Category")
                        .WithMany("Cars")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("DataAccess.Entities.CarBooking", b =>
                {
                    b.HasOne("DataAccess.Entities.Booking", "Booking")
                        .WithOne("CarBooking")
                        .HasForeignKey("DataAccess.Entities.CarBooking", "BookingId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("DataAccess.Entities.Car", "Car")
                        .WithMany("CarBookings")
                        .HasForeignKey("CarId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Booking");

                    b.Navigation("Car");
                });

            modelBuilder.Entity("DataAccess.Entities.Customer", b =>
                {
                    b.HasOne("DataAccess.Entities.User", "User")
                        .WithOne()
                        .HasForeignKey("DataAccess.Entities.Customer", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("DataAccess.Entities.Employee", b =>
                {
                    b.HasOne("DataAccess.Entities.User", "User")
                        .WithOne()
                        .HasForeignKey("DataAccess.Entities.Employee", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("DataAccess.Entities.ImageShot", b =>
                {
                    b.HasOne("DataAccess.Entities.CarBooking", "CarBooking")
                        .WithMany("ImageShots")
                        .HasForeignKey("CarBookingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CarBooking");
                });

            modelBuilder.Entity("DataAccess.Entities.Payment", b =>
                {
                    b.HasOne("DataAccess.Entities.Booking", "Booking")
                        .WithMany("Payments")
                        .HasForeignKey("BookingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Booking");
                });

            modelBuilder.Entity("DataAccess.Entities.PaymentTransaction", b =>
                {
                    b.HasOne("DataAccess.Entities.Payment", "Payment")
                        .WithMany("Transactions")
                        .HasForeignKey("PaymentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataAccess.Entities.PaymentMethod", "PaymentMethod")
                        .WithMany("PaymentTransactions")
                        .HasForeignKey("PaymentMethodId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Payment");

                    b.Navigation("PaymentMethod");
                });

            modelBuilder.Entity("DataAccess.Entities.Post", b =>
                {
                    b.HasOne("DataAccess.Entities.Employee", "Employee")
                        .WithMany("Posts")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("DataAccess.Entities.PostType", "PostType")
                        .WithMany("Posts")
                        .HasForeignKey("PostTypeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Employee");

                    b.Navigation("PostType");
                });

            modelBuilder.Entity("DataAccess.Entities.PostTag", b =>
                {
                    b.HasOne("DataAccess.Entities.Post", "Post")
                        .WithMany("PostTags")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataAccess.Entities.Tag", "Tag")
                        .WithMany("PostTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");

                    b.Navigation("Tag");
                });

            modelBuilder.Entity("DataAccess.Entities.SEOMetadata", b =>
                {
                    b.HasOne("DataAccess.Entities.Post", "Post")
                        .WithMany("SEOMetadata")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");
                });

            modelBuilder.Entity("DataAccess.Entities.TripBooking", b =>
                {
                    b.HasOne("DataAccess.Entities.Booking", "Booking")
                        .WithOne("TripBooking")
                        .HasForeignKey("DataAccess.Entities.TripBooking", "BookingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataAccess.Entities.TripPlan", "TripPlan")
                        .WithMany("Bookings")
                        .HasForeignKey("TripPlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Booking");

                    b.Navigation("TripPlan");
                });

            modelBuilder.Entity("DataAccess.Entities.TripPlan", b =>
                {
                    b.HasOne("DataAccess.Entities.Region", "Region")
                        .WithMany("Plans")
                        .HasForeignKey("RegionId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("DataAccess.Entities.Trip", "Trip")
                        .WithMany("Plans")
                        .HasForeignKey("TripId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Region");

                    b.Navigation("Trip");
                });

            modelBuilder.Entity("DataAccess.Entities.TripPlanCar", b =>
                {
                    b.HasOne("DataAccess.Entities.Car", "Car")
                        .WithMany("TripPlanCars")
                        .HasForeignKey("CarId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("DataAccess.Entities.TripPlan", "TripPlan")
                        .WithMany("PlanCars")
                        .HasForeignKey("TripPlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Car");

                    b.Navigation("TripPlan");
                });

            modelBuilder.Entity("DataAccess.Entities.Booking", b =>
                {
                    b.Navigation("CarBooking");

                    b.Navigation("Payments");

                    b.Navigation("TripBooking");
                });

            modelBuilder.Entity("DataAccess.Entities.Car", b =>
                {
                    b.Navigation("CarBookings");

                    b.Navigation("TripPlanCars");
                });

            modelBuilder.Entity("DataAccess.Entities.CarBooking", b =>
                {
                    b.Navigation("ImageShots");
                });

            modelBuilder.Entity("DataAccess.Entities.Category", b =>
                {
                    b.Navigation("Cars");
                });

            modelBuilder.Entity("DataAccess.Entities.Customer", b =>
                {
                    b.Navigation("Bookings");
                });

            modelBuilder.Entity("DataAccess.Entities.Employee", b =>
                {
                    b.Navigation("Bookings");

                    b.Navigation("Posts");
                });

            modelBuilder.Entity("DataAccess.Entities.Payment", b =>
                {
                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("DataAccess.Entities.PaymentMethod", b =>
                {
                    b.Navigation("PaymentTransactions");
                });

            modelBuilder.Entity("DataAccess.Entities.Post", b =>
                {
                    b.Navigation("PostTags");

                    b.Navigation("SEOMetadata");
                });

            modelBuilder.Entity("DataAccess.Entities.PostType", b =>
                {
                    b.Navigation("Posts");
                });

            modelBuilder.Entity("DataAccess.Entities.Region", b =>
                {
                    b.Navigation("Plans");
                });

            modelBuilder.Entity("DataAccess.Entities.Tag", b =>
                {
                    b.Navigation("PostTags");
                });

            modelBuilder.Entity("DataAccess.Entities.Trip", b =>
                {
                    b.Navigation("Plans");
                });

            modelBuilder.Entity("DataAccess.Entities.TripPlan", b =>
                {
                    b.Navigation("Bookings");

                    b.Navigation("PlanCars");
                });
#pragma warning restore 612, 618
        }
    }
}
