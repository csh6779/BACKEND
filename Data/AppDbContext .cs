using Microsoft.EntityFrameworkCore;
using RigidboysAPI.Dtos;
using RigidboysAPI.Models;

namespace RigidboysAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Purchase> Purchase { get; set; }

        // ✅ 클래스 내부에 OnModelCreating 추가
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.OfficeName)
                .IsUnique();  // ✅ OfficeName 중복 방지
            modelBuilder.Entity<Product>()
                .HasIndex(p => p.Product_name)
                .IsUnique();
            modelBuilder.Entity<Purchase>()
           .HasIndex(p => p.OfficeName)
           .IsUnique();
        }
    }
}
