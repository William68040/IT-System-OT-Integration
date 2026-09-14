using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Shared.Models;
using Shared.Enums;

namespace Shared.Data
{
    public class FactoryDbContext : DbContext
    {
        public FactoryDbContext(DbContextOptions<FactoryDbContext> options)
            : base(options)
        {
        }

        // custructor for design-time DbContext creation (e.g., for migrations)
        public FactoryDbContext()
        {
        }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<ProductionJob> ProductionJobs => Set<ProductionJob>();
        public DbSet<User> Users => Set<User>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    "Server=(localdb)\\MSSQLLocalDB;Database=JewelryFactoryDb;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Product (1) -> Orders (Many)
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Product)
                .WithMany(p => p.Orders)
                .HasForeignKey(o => o.ProductId);

            // Order (1) -> ProductionJob (1)
            modelBuilder.Entity<Order>()
                .HasOne(o => o.ProductionJob)
                .WithOne(j => j.Order)
                .HasForeignKey<ProductionJob>(j => j.OrderId);
        }
    }
}