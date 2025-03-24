using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DataAccess.Entities
{

    public partial class TourismAgencyDbContext : DbContext
    {
        public TourismAgencyDbContext(DbContextOptions<TourismAgencyDbContext> options)
            : base(options)
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
            modelBuilder.Entity<Payment>(entity => {
                entity.ToTable("Payments");
                entity.HasMany(p => p.Transactions)
                      .WithOne()
                      .HasForeignKey(pt => pt.PaymentId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.Property(p=>p.Status)
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
            });
        }
        private static void ConfigurePost(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("Posts");
                entity.Property(e=>e.Status)
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
        }
        private static void ConfigureRegion(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Region>().ToTable("Regions")
                        .HasIndex(r => r.Name).IsUnique();;
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
    }
}