using FunctionCallingFeature.Models.EShop;
using Microsoft.EntityFrameworkCore;

namespace FunctionCallingFeature.Data
{
    public class EShopDbContext : DbContext
    {
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderProduct> OrderProducts => Set<OrderProduct>();

        public EShopDbContext(DbContextOptions options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderProduct>().HasKey(op => new { op.OrderId, op.ProductId });
            modelBuilder.Entity<Product>().HasData(DataSeeder.GetProductList());
            base.OnModelCreating(modelBuilder);
        }
    }
}
