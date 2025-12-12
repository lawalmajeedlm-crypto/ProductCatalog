using Microsoft.EntityFrameworkCore;
using ProductCatalog.Entities;
using ProductCatalog.Models;

namespace ProductCatalog.Data
{
    public class CatalogDbContext : DbContext
    {
        public CatalogDbContext(DbContextOptions<CatalogDbContext> options)
           : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = default!;
        public DbSet<Product> Products { get; set; } = default!;
        public DbSet<Order> Orders { get; set; } = default!;
        public DbSet<OrderItem> OrderItems { get; set; } = default!;
        public DbSet<Cart> Carts { get; set; } = default!;
        public DbSet<CartItem> CartItems { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasQueryFilter(u => !u.IsDeleted);
            modelBuilder.Entity<Product>().HasQueryFilter(p => !p.IsDeleted);
            modelBuilder.Entity<Order>().HasQueryFilter(o => !o.IsDeleted);
            modelBuilder.Entity<OrderItem>().HasQueryFilter(oi => !oi.IsDeleted);
            modelBuilder.Entity<Cart>().HasQueryFilter(c => !c.IsDeleted);
            modelBuilder.Entity<CartItem>().HasQueryFilter(ci => !ci.IsDeleted);

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Name).IsRequired().HasMaxLength(200);
                entity.Property(p => p.Price).HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(o => o.Id);
                entity.Property(o => o.Status).IsRequired().HasMaxLength(50);
                entity.Property(o => o.TotalAmount).HasColumnType("decimal(18,2)");

                entity.HasOne(o => o.User)
                      .WithMany(u => u.Orders)
                      .HasForeignKey(o => o.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(oi => oi.Id);
                entity.Property(oi => oi.UnitPrice).HasColumnType("decimal(18,2)");
                entity.Property(oi => oi.Quantity).IsRequired();

                entity.HasOne(oi => oi.Order)
                      .WithMany(o => o.Items)
                      .HasForeignKey(oi => oi.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(oi => oi.Product)
                      .WithMany()
                      .HasForeignKey(oi => oi.ProductId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Cart>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.HasOne(c => c.User)
                      .WithMany(u => u.Carts)
                      .HasForeignKey(c => c.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.Property(c =>c.TotalAmount)
                        .HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.HasKey(ci => ci.Id);
                entity.Property(ci => ci.UnitPrice).HasColumnType("decimal(18,2)");
                entity.Property(ci => ci.Quantity).IsRequired();

                entity.HasOne(ci => ci.Cart)
                      .WithMany(c => c.Items)
                      .HasForeignKey(ci => ci.CartId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ci => ci.Product)
                      .WithMany()
                      .HasForeignKey(ci => ci.ProductId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.Email).IsRequired().HasMaxLength(150);
                entity.Property(u => u.FullName).IsRequired().HasMaxLength(200);
                entity.Property(u => u.PasswordHash).IsRequired();
                entity.Property(u => u.Role).HasMaxLength(50);
            });
        }
    }
}
