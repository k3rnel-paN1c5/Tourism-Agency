using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Infrastructure.DataSeeders;

namespace Infrastructure.Contexts;

/// <summary>
/// Represents the database context for the Tourism Agency application.
/// Manages the relationships and configurations for all domain entities.
/// </summary>
public partial class TourismAgencyDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TourismAgencyDbContext"/> class with specified options.
    /// </summary>
    /// <param name="options">The options to be used by this context.</param>
    public TourismAgencyDbContext(DbContextOptions<TourismAgencyDbContext> options)
        : base(options)
    {

    }
    /// <summary>
    /// Initializes a new instance of the <see cref="TourismAgencyDbContext"/> class.
    /// This constructor can be used for design-time tooling or when options are set elsewhere.
    /// </summary>
    public TourismAgencyDbContext()
    {
    }
    // DbSets for each entity
    /// <summary>
    /// Gets or sets the DbSet for <see cref="Category"/> entities.
    /// </summary>
    public DbSet<Category> Categories { get; set; }
    /// <summary>
    /// Gets or sets the DbSet for <see cref="Car"/> entities.
    /// </summary>
    public DbSet<Car> Cars { get; set; }
    /// <summary>
    /// Gets or sets the DbSet for <see cref="Payment"/> entities.
    /// </summary>
    public DbSet<Payment> Payments { get; set; }
    /// <summary>
    /// Gets or sets the DbSet for <see cref="PaymentMethod"/> entities.
    /// </summary>
    public DbSet<PaymentMethod> PaymentMethods { get; set; }
    /// <summary>
    /// Gets or sets the DbSet for <see cref="PaymentTransaction"/> entities.
    /// </summary>
    public DbSet<PaymentTransaction> PaymentTransactions { get; set; }
    /// <summary>
    /// Gets or sets the DbSet for <see cref="Post"/> entities.
    /// </summary>
    public DbSet<Post> Posts { get; set; }
    /// <summary>
    /// Gets or sets the DbSet for <see cref="PostTag"/> entities.
    /// </summary>
    public DbSet<PostTag> PostTags { get; set; }
    /// <summary>
    /// Gets or sets the DbSet for <see cref="PostType"/> entities.
    /// </summary>
    public DbSet<PostType> PostTypes { get; set; }
    /// <summary>
    /// Gets or sets the DbSet for <see cref="Region"/> entities.
    /// </summary>
    public DbSet<Region> Regions { get; set; }
    /// <summary>
    /// Gets or sets the DbSet for <see cref="SEOMetadata"/> entities.
    /// </summary>
    public DbSet<SEOMetadata> SEOMetadatas { get; set; }
    /// <summary>
    /// Gets or sets the DbSet for <see cref="TripBooking"/> entities.
    /// </summary>
    public DbSet<TripBooking> TripBookings { get; set; }
    /// <summary>
    /// Gets or sets the DbSet for <see cref="TripPlanCar"/> entities.
    /// </summary>
    public DbSet<TripPlanCar> TripPlanCars { get; set; }
    /// <summary>
    /// Gets or sets the DbSet for <see cref="TripPlan"/> entities.
    /// </summary>
    public DbSet<TripPlan> TripPlans { get; set; }
    /// <summary>
    /// Gets or sets the DbSet for <see cref="Trip"/> entities.
    /// </summary>
    public DbSet<Trip> Trips { get; set; }
    /// <summary>
    /// Gets or sets the DbSet for <see cref="CarBooking"/> entities.
    /// </summary>
    public DbSet<CarBooking> CarBookings { get; set; }
    /// <summary>
    /// Gets or sets the DbSet for <see cref="Booking"/> entities.
    /// </summary>
    public DbSet<Booking> Bookings { get; set; }
    /// <summary>
    /// Gets or sets the DbSet for <see cref="ImageShot"/> entities.
    /// </summary>
    public DbSet<ImageShot> ImageShots { get; set; }
    /// <summary>
    /// Gets or sets the DbSet for <see cref="Customer"/> entities.
    /// </summary>
    public DbSet<Customer> Customers { get; set; }
    /// <summary>
    /// Gets or sets the DbSet for <see cref="Employee"/> entities.
    /// </summary>
    public DbSet<Employee> Employees { get; set; }


    /// <summary>
    /// Configures the model that was discovered by convention from the entity types exposed in <see cref="DbSet{TEntity}"/> properties.
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureCar(modelBuilder);
        ConfigureCategory(modelBuilder);
        ConfigurePayment(modelBuilder);
        ConfigurePaymentMethod(modelBuilder);
        ConfigurePaymentTransaction(modelBuilder);
        ConfigurePost(modelBuilder);
        ConfigurePostTag(modelBuilder);
        ConfigurePostType(modelBuilder);
        ConfigureRegion(modelBuilder);
        ConfigureSEOMetadata(modelBuilder);
        ConfigureTag(modelBuilder);
        ConfigureTrip(modelBuilder);
        ConfigureCarBooking(modelBuilder);
        ConfigureBooking(modelBuilder);
        ConfigureImageShot(modelBuilder);
        ConfigureTripBooking(modelBuilder);
        ConfigureTripPlan(modelBuilder);
        ConfigureTripPlanCar(modelBuilder);
        ConfigureCustomer(modelBuilder);
        ConfigureEmployee(modelBuilder);

    }

    /// <summary>
    /// Configures the <see cref="Car"/> entity's properties and relationships.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    private static void ConfigureCar(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Car>().ToTable("Cars")
                    .HasOne(c => c.Category)
                    .WithMany(ct => ct.Cars)
                    .HasForeignKey(c => c.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
        // Seed cars
        modelBuilder.Entity<Car>().HasData(SeedData.GetCars());
    }

    /// <summary>
    /// Configures the <see cref="Category"/> entity's properties and relationships.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    private static void ConfigureCategory(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().ToTable("Categories")
                    .HasIndex(c => c.Title).IsUnique();
        // Seed categories
        modelBuilder.Entity<Category>().HasData(SeedData.GetCategories());
    }

    /// <summary>
    /// Configures the <see cref="Payment"/> entity's properties and relationships.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    private static void ConfigurePayment(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Payment>(entity =>
        {
            entity.ToTable("Payments");
            entity.HasMany(p => p.Transactions)
                  .WithOne(t => t.Payment)
                  .HasForeignKey(pt => pt.PaymentId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.Property(p => p.Status)
                  .HasConversion<string>()
                  .HasColumnType("nvarchar(30)");
        });
    }

    /// <summary>
    /// Configures the <see cref="PaymentMethod"/> entity's properties and relationships.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    private static void ConfigurePaymentMethod(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PaymentMethod>().ToTable("PaymentMethods")
                    .HasMany(pm => pm.PaymentTransactions)
                    .WithOne(pt => pt.PaymentMethod)
                    .HasForeignKey(pt => pt.PaymentMethodId)
                    .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PaymentMethod>().HasIndex(e => e.Method).IsUnique();
    }

    /// <summary>
    /// Configures the <see cref="PaymentTransaction"/> entity's properties and relationships.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    private static void ConfigurePaymentTransaction(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PaymentTransaction>(entity =>
        {
            entity.ToTable("PaymentTransaction");
            entity.Property(e => e.TransactionType)
                  .HasConversion<string>()
                  .HasColumnType("nvarchar(30)");

            entity.HasOne(pt => pt.Payment)
                 .WithMany(p => p.Transactions)
                 .HasForeignKey(pt => pt.PaymentId)
                 .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(pt => pt.PaymentMethod)
                 .WithMany(pm => pm.PaymentTransactions)
                 .HasForeignKey(pt => pt.PaymentMethodId)
                 .OnDelete(DeleteBehavior.Restrict);
            entity.HasIndex(e => new { e.PaymentId, e.PaymentMethodId, e.TransactionDate })
            .IsUnique(true);
        });
    }

    /// <summary>
    /// Configures the <see cref="Post"/> entity's properties and relationships.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    private static void ConfigurePost(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Post>(entity =>
        {
            entity.ToTable("Posts");

            entity.Property(e => e.Status)
                  .HasConversion<string>()
                  .HasColumnType("nvarchar(30)");

            entity.HasOne(p => p.PostType)
                  .WithMany(pt => pt.Posts)
                  .HasForeignKey(p => p.PostTypeId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(p => p.PostTags)
                  .WithOne(pt => pt.Post)
                  .HasForeignKey(pt => pt.PostId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(p => p.Employee)
                  .WithMany(e => e.Posts)
                  .HasForeignKey(p => p.EmployeeId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }

    /// <summary>
    /// Configures the <see cref="PostTag"/> entity's properties and relationships.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    private static void ConfigurePostTag(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PostTag>(entity =>
        {
            entity.ToTable("PostTags")
                  .HasKey(pt => new { pt.PostId, pt.TagId });

            entity.HasOne(pt => pt.Post)
                 .WithMany(p => p.PostTags)
                 .HasForeignKey(pt => pt.PostId)
                 .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(pt => pt.Tag)
                 .WithMany(t => t.PostTags)
                 .HasForeignKey(pt => pt.TagId)
                 .OnDelete(DeleteBehavior.Cascade);
        });
    }

    /// <summary>
    /// Configures the <see cref="PostType"/> entity's properties and relationships.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    private static void ConfigurePostType(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PostType>().ToTable("PostTypes");
        modelBuilder.Entity<PostType>().HasIndex(e => e.Title).IsUnique();

    }

    /// <summary>
    /// Configures the <see cref="Region"/> entity's properties and relationships.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    private static void ConfigureRegion(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Region>().ToTable("Regions")
                    .HasIndex(r => r.Name).IsUnique(); ;
    }

    /// <summary>
    /// Configures the <see cref="SEOMetadata"/> entity's properties and relationships.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    private static void ConfigureSEOMetadata(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SEOMetadata>(entity =>
        {
            entity.ToTable("SEOMetadata");

            entity.HasOne(seo => seo.Post)
                 .WithMany(p => p.SEOMetadata)
                 .HasForeignKey(seo => seo.PostId)
                 .OnDelete(DeleteBehavior.Cascade);
        });
    }

    /// <summary>
    /// Configures the <see cref="Tag"/> entity's properties and relationships.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    private static void ConfigureTag(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tag>(entity =>
        {
            entity.ToTable("Tags");

            entity.HasMany(t => t.PostTags)
                 .WithOne(pt => pt.Tag)
                 .HasForeignKey(pt => pt.TagId)
                 .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(t => t.Name).IsUnique();
        });
    }

    /// <summary>
    /// Configures the <see cref="Trip"/> entity's properties and relationships.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    private static void ConfigureTrip(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Trip>(entity =>
        {
            entity.ToTable("Trips");

            entity.HasIndex(t => t.Name).IsUnique();
            entity.HasIndex(t => t.Slug).IsUnique();
        });
    }

    /// <summary>
    /// Configures the <see cref="CarBooking"/> entity's properties and relationships.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    private static void ConfigureCarBooking(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CarBooking>(entity =>
        {
            entity.ToTable("CarBookings");
            entity.HasKey(cb => cb.BookingId);
            entity.HasOne(cb => cb.Car)
            .WithMany(c => c.CarBookings)
            .HasForeignKey(cb => cb.CarId)
            .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(cb => cb.Booking)
            .WithOne(b => b.CarBooking)
            .HasForeignKey<CarBooking>(cb => cb.BookingId)
            .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(cb => cb.ImageShots)
            .WithOne(im => im.CarBooking)
            .HasForeignKey(im => im.CarBookingId)
            .OnDelete(DeleteBehavior.Cascade);
        });
    }

    /// <summary>
    /// Configures the <see cref="Booking"/> entity's properties and relationships.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    private static void ConfigureBooking(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.ToTable("Bookings");
            entity.Property(b => b.Status)
                  .HasConversion<string>()
                  .HasColumnType("nvarchar(30)");


            entity.HasKey(b => b.Id);

            entity.HasOne(b => b.CarBooking)
                  .WithOne(cb => cb.Booking)
                  .HasForeignKey<CarBooking>(cb => cb.BookingId)
                  .OnDelete(DeleteBehavior.Restrict);


            entity.HasOne(b => b.TripBooking)
                  .WithOne(tb => tb.Booking)
                  .HasForeignKey<TripBooking>(tb => tb.BookingId)
                  .OnDelete(DeleteBehavior.Restrict);


            entity.HasOne(b => b.Payment)
                  .WithOne(p => p.Booking)
                  .HasForeignKey<Payment>(p => p.BookingId)
                  .IsRequired(true)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(b => b.Employee)
                  .WithMany(p => p.Bookings)
                  .HasForeignKey(p => p.EmployeeId)
                  .IsRequired(false)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(b => b.Customer)
                  .WithMany(p => p.Bookings)
                  .HasForeignKey(p => p.CustomerId)
                  .OnDelete(DeleteBehavior.Restrict);

        });
    }

    /// <summary>
    /// Configures the <see cref="ImageShot"/> entity's properties and relationships.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    private static void ConfigureImageShot(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ImageShot>(entity =>
        {
            entity.ToTable("ImageShots");

            entity.HasKey(i => i.Id);

            entity.HasOne(i => i.CarBooking)
                  .WithMany(cb => cb.ImageShots)
                  .HasForeignKey(i => i.CarBookingId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }

    /// <summary>
    /// Configures the <see cref="TripBooking"/> entity's properties and relationships.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>

    private static void ConfigureTripBooking(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TripBooking>(entity =>
        {
            entity.ToTable("TripBookings");
            entity.HasKey(t => t.BookingId);


            entity.HasOne(t => t.Booking)
                  .WithOne(b => b.TripBooking)
                  .HasForeignKey<TripBooking>(t => t.BookingId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(t => t.TripPlan)
                  .WithMany(tp => tp.Bookings)
                  .HasForeignKey(t => t.TripPlanId)
                  .OnDelete(DeleteBehavior.Restrict);

        });
    }

    /// <summary>
    /// Configures the <see cref="TripPlan"/> entity's properties and relationships.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    private static void ConfigureTripPlan(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TripPlan>(entity =>
        {
            entity.ToTable("TripPlans");

            entity.HasKey(tp => tp.Id);

            entity.HasOne(tp => tp.Region)
                  .WithMany(r => r.Plans)
                  .HasForeignKey(tp => tp.RegionId)
                  .OnDelete(DeleteBehavior.Restrict);


            entity.HasOne(tp => tp.Trip)
                  .WithMany(t => t.Plans)
                  .HasForeignKey(tp => tp.TripId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(tp => tp.Bookings)
                  .WithOne(b => b.TripPlan)
                  .HasForeignKey(tb => tb.TripPlanId)
                  .OnDelete(DeleteBehavior.Cascade);


            entity.HasMany(tp => tp.PlanCars)
                  .WithOne(pc => pc.TripPlan)
                  .HasForeignKey(tpc => tpc.TripPlanId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }

    /// <summary>
    /// Configures the <see cref="TripPlanCar"/> entity's properties and relationships.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    private static void ConfigureTripPlanCar(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TripPlanCar>(entity =>
        {
            entity.ToTable("TripPlanCars");

            entity.HasKey(tpc => tpc.Id);

            entity.HasOne(tpc => tpc.TripPlan)
                  .WithMany(tp => tp.PlanCars)
                  .HasForeignKey(tpc => tpc.TripPlanId)
                  .OnDelete(DeleteBehavior.Cascade);


            entity.HasOne(tpc => tpc.Car)
                  .WithMany(c => c.TripPlanCars)
                  .HasForeignKey(tpc => tpc.CarId)
                  .OnDelete(DeleteBehavior.Restrict);

        });
    }

    /// <summary>
    /// Configures the <see cref="Employee"/> entity's properties and relationships, including data seeding.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    private static void ConfigureEmployee(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(static entity =>
        {
            entity.ToTable("Employees");
        });
    }
    /// <summary>
    /// Configures the <see cref="Customer"/> entity's properties and relationships, including data seeding.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    private static void ConfigureCustomer(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(static entity =>
        {
            entity.ToTable("Customers");
        });
    }
}
