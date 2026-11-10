namespace taskk7B.Models
{
    using Microsoft.EntityFrameworkCore;

    public class Task7Context : DbContext
    {
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<Payment> Payments => Set<Payment>();

        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server=.;Database=task7_CodeFirst;" +
                "Trusted_Connection=True;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // العملاء
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(x => x.FullName)
                    .HasMaxLength(100).IsRequired();

                entity.Property(x => x.Email)
                    .HasMaxLength(150).IsRequired();

                entity.Property(x => x.Phone).HasMaxLength(20);

                entity.HasIndex(x => x.Email).IsUnique();
            });

            // المنتجات
            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(x => x.ProductName)
                    .HasMaxLength(100).IsRequired();

                entity.Property(x => x.Price).HasPrecision(10, 2);

                entity.Property(x => x.StockQuantity).HasDefaultValue(0);

                entity.ToTable("Products", table =>
                {
                    table.HasCheckConstraint(
                        "CK_Products_Price", "[Price] >= 0");

                    table.HasCheckConstraint(
                        "CK_Products_Stock", "[StockQuantity] >= 0");
                });
            });

            // الطلبات وعلاقتها بالعملاء
            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(x => x.OrderDate)
                    .HasDefaultValueSql("SYSDATETIME()");

                entity.Property(x => x.OrderStatus)
                    .HasMaxLength(20)
                    .IsRequired()
                    .HasDefaultValue("Pending");

                entity.HasOne(x => x.Customer)
                    .WithMany(x => x.Orders)
                    .HasForeignKey(x => x.CustomerID)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.ToTable("Orders", table =>
                    table.HasCheckConstraint(
                        "CK_Orders_Status",
                        "[OrderStatus] IN " +
                        "('Pending','Processing','Completed','Cancelled')"));
            });

            // تفاصيل الطلب والمفتاح المركب
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(x => new { x.OrderID, x.ProductID });

                entity.Property(x => x.UnitPrice).HasPrecision(10, 2);

                entity.HasOne(x => x.Order)
                    .WithMany(x => x.OrderItems)
                    .HasForeignKey(x => x.OrderID)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(x => x.Product)
                    .WithMany(x => x.OrderItems)
                    .HasForeignKey(x => x.ProductID)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.ToTable("OrderItems", table =>
                {
                    table.HasCheckConstraint(
                        "CK_OrderItems_Quantity", "[Quantity] > 0");

                    table.HasCheckConstraint(
                        "CK_OrderItems_Price", "[UnitPrice] >= 0");
                });
            });

            // الدفعات وعلاقتها بالطلبات
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.Property(x => x.Amount).HasPrecision(10, 2);

                entity.Property(x => x.PaymentDate)
                    .HasDefaultValueSql("SYSDATETIME()");

                entity.Property(x => x.PaymentMethod)
                    .HasMaxLength(20).IsRequired();

                entity.HasOne(x => x.Order)
                    .WithMany(x => x.Payments)
                    .HasForeignKey(x => x.OrderID)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.ToTable("Payments", table =>
                {
                    table.HasCheckConstraint(
                        "CK_Payments_Amount", "[Amount] > 0");

                    table.HasCheckConstraint(
                        "CK_Payments_Method",
                        "[PaymentMethod] IN ('Cash','Card','BankTransfer')");
                });
            });
        }
    }
}
