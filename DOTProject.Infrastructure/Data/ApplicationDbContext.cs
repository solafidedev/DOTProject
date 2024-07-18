using DOTProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DOTProject.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.Price)
                      .HasColumnType("decimal(18,2)");

                entity.HasOne(p => p.Category)
                      .WithMany(c => c.Products)
                      .HasForeignKey(p => p.CategoryId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(100);
            });

            // Seed data
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Electronics" },
                new Category { Id = 2, Name = "Books" }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Laptop", Price = 15000000, CategoryId = 1 },
                new Product { Id = 2, Name = "Headphones", Price = 5000000, CategoryId = 1 },
                new Product { Id = 3, Name = "Science Fiction Book", Price = 350000, CategoryId = 2 }
            );
        }
    }
}