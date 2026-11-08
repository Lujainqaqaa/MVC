
using Microsoft.EntityFrameworkCore;
using UniversityCafeteria.Models.CodeFirst;

namespace UniversityCafeteria.Data
{
    public class CafeteriaEmployeesDbContext : DbContext
    {
        public CafeteriaEmployeesDbContext(
            DbContextOptions<CafeteriaEmployeesDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Supplier)
                .WithMany(s => s.Products)
                .HasForeignKey(p => p.SupplierId);

            modelBuilder.Entity<PurchaseOrder>()
                .HasOne(o => o.Supplier)
                .WithMany(s => s.PurchaseOrders)
                .HasForeignKey(o => o.SupplierId);

            modelBuilder.Entity<PurchaseOrderItem>()
                .HasOne(i => i.PurchaseOrder)
                .WithMany(o => o.Items)
                .HasForeignKey(i => i.PurchaseOrderId);

            modelBuilder.Entity<PurchaseOrderItem>()
                .HasOne(i => i.Product)
                .WithMany(p => p.PurchaseOrderItems)
                .HasForeignKey(i => i.ProductId);
        }
    }
}
