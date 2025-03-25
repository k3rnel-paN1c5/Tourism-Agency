using Microsoft.EntityFrameworkCore;
namespace DataAccess.Entities
{

    public partial class TourismAgencyDbContext : DbContext
    {
        public TourismAgencyDbContext(DbContextOptions<TourismAgencyDbContext> options)
            : base(options)
        {

        }
        public TourismAgencyDbContext()
        {
        }
        // DbSets for each entity
        public DbSet<Category> Categories { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<PaymentTransaction> PaymentTransactions { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostTag> PostTags { get; set; }
        public DbSet<PostType> PostTypes { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<SEOMetadata> SEOMetadatas { get; set; }
        public DbSet<TripBooking> TripBookings { get; set; }
        public DbSet<TripPlanCar> TripPlanCars { get; set; }
        public DbSet<TripPlan> TripPlans { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<CarBooking> CarBookings { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<ImageShot> ImageShots { get; set; }



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

        }

        private static void ConfigureCar(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Car>().ToTable("Cars")
                        .HasOne(c => c.Category)
                        .WithMany()
                        .HasForeignKey(c => c.CategoryId)
                        .OnDelete(DeleteBehavior.Restrict);
        }

        private static void ConfigureCategory(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().ToTable("Categories")
                        .HasIndex(c => c.Title).IsUnique();
        }
        private static void ConfigurePayment(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payments");
                entity.HasMany(p => p.Transactions)
                      .WithOne()
                      .HasForeignKey(pt => pt.PaymentId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.Property(p => p.Status)
                      .HasConversion<string>();
            });
        }

        private static void ConfigurePaymentMethod(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PaymentMethod>().ToTable("PaymentMethods")
                        .HasMany(pm => pm.PaymentTransactions)
                        .WithOne()
                        .HasForeignKey(pt => pt.PaymentMethodId)
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PaymentMethod>().HasIndex(e => e.Method).IsUnique();
        }
        private static void ConfigurePaymentTransaction(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PaymentTransaction>(entity =>
            {
                entity.ToTable("PaymentTransaction");
                entity.Property(e => e.TransactionType)
                      .HasConversion<string>()
                      .HasColumnType("nvarchar(10)");

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
        private static void ConfigurePost(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("Posts");
                entity.Property(e => e.Status)
                      .HasConversion<string>();
                entity.HasOne(p => p.PostType)
                     .WithMany()
                     .HasForeignKey(p => p.PostTypeId)
                     .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(p => p.PostTags)
                     .WithOne(pt => pt.Post)
                     .HasForeignKey(pt => pt.PostId)
                     .OnDelete(DeleteBehavior.Cascade);
            });
        }
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

        private static void ConfigurePostType(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PostType>().ToTable("PostTypes");
            modelBuilder.Entity<PostType>().HasIndex(e => e.Title).IsUnique();

        }
        private static void ConfigureRegion(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Region>().ToTable("Regions")
                        .HasIndex(r => r.Name).IsUnique(); ;
        }

        private static void ConfigureSEOMetadata(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SEOMetadata>(entity =>
            {
                entity.ToTable("SEOMetadata");

                entity.HasOne(seo => seo.Post)
                     .WithMany()
                     .HasForeignKey(seo => seo.PostId)
                     .OnDelete(DeleteBehavior.Cascade);
            });
        }
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

        private static void ConfigureTrip(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Trip>(entity =>
            {
                entity.ToTable("Trips");

                entity.HasIndex(t => t.Name).IsUnique();
                entity.HasIndex(t => t.Slug).IsUnique();
            });
        }
        private static void ConfigureCarBooking(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CarBooking>(entity =>
            {
                entity.ToTable("CarBookings");
                entity.HasKey(cb => cb.BookingId);
                entity.HasOne(cb => cb.Car)
                .WithMany()
                .HasForeignKey(cb => cb.CarId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(cb => cb.Booking)
              .WithOne()
              .HasForeignKey<CarBooking>(cb => cb.BookingId)
              .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(cb => cb.ImageShots)
             .WithOne()
             .HasForeignKey(im => im.CarBookingId)
             .OnDelete(DeleteBehavior.Cascade);

            });
        }
        private static void ConfigureBooking(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.ToTable("Bookings");


                entity.HasKey(b => b.Id);

                entity.HasOne(b => b.CarBooking)
                      .WithOne(cb => cb.Booking)
                      .HasForeignKey<CarBooking>(cb => cb.BookingId)
                      .OnDelete(DeleteBehavior.Restrict);


                entity.HasOne(b => b.TripBooking)
                      .WithOne(tb => tb.Booking)
                      .HasForeignKey<TripBooking>(tb => tb.BookingId)
                      .OnDelete(DeleteBehavior.Restrict);


                entity.HasMany(b => b.Payments)
                      .WithOne()
                      .HasForeignKey(p => p.BookingId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
        private static void ConfigureImageShot(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ImageShot>(entity =>
            {
                entity.ToTable("ImageShots");

                entity.HasKey(i => i.Id);

                entity.HasOne(i => i.CarBooking)
                      .WithMany()
                      .HasForeignKey(i => i.CarBookingId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
        private static void ConfigureTripBooking(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TripBooking>(entity =>
            {
                entity.ToTable("TripBookings");
                entity.HasKey(t => t.BookingId);


                entity.HasOne(t => t.Booking)
                      .WithOne()
                      .HasForeignKey<TripBooking>(t => t.BookingId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(t => t.TripPlan)
                      .WithMany()
                      .HasForeignKey(t => t.TripPlanId)
                      .OnDelete(DeleteBehavior.Restrict);

            });
        }
        private static void ConfigureTripPlan(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TripPlan>(entity =>
            {
                entity.ToTable("TripPlans");

                entity.HasKey(tp => tp.Id);

                entity.HasOne(tp => tp.Region)
                      .WithMany()
                      .HasForeignKey(tp => tp.RegionId)
                      .OnDelete(DeleteBehavior.Restrict);


                entity.HasOne(tp => tp.Trip)
                      .WithMany()
                      .HasForeignKey(tp => tp.TripId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(tp => tp.Bookings)
                      .WithOne()
                      .HasForeignKey(tb => tb.TripPlanId)
                      .OnDelete(DeleteBehavior.Cascade);


                entity.HasMany(tp => tp.PlanCars)
                      .WithOne()
                      .HasForeignKey(tpc => tpc.TripPlanId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
        private static void ConfigureTripPlanCar(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TripPlanCar>(entity =>
            {
                entity.ToTable("TripPlanCars");

                entity.HasKey(tpc => tpc.Id);

                entity.HasOne(tpc => tpc.TripPlan) 
                      .WithMany()
                      .HasForeignKey(tpc => tpc.TripPlanId)  
                      .OnDelete(DeleteBehavior.Cascade);  

                
                entity.HasOne(tpc => tpc.Car)  
                      .WithMany(c => c.TripPlanCars) 
                      .HasForeignKey(tpc => tpc.CarId) 
                      .OnDelete(DeleteBehavior.Restrict);

            });
        }
    }
}