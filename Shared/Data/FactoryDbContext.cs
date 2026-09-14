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


            // seeding inital data för products
            modelBuilder.Entity<Product>().HasData(
                new Product { 
                Id = 1, 
                Name = "Silver Necklace",
                Description = "925 sterling silver necklace",
                MachineType = MachineType.Necklace ,
                ProductionTime = 10,
                IsActive = true,
                },

                new Product
                {
                    Id = 2,
                    Name = "Silver Ring",
                    Description = "925 sterling silver ring",
                    MachineType = MachineType.Ring ,
                    ProductionTime = 12,
                    IsActive = true,
                },

                new Product
                {
                    Id = 3,
                    Name = "Silver Bracelet",
                    Description = "925 sterling silver bracelet",
                    MachineType = MachineType.Bracelet ,
                    ProductionTime = 7,
                    IsActive = true,
                },

                new Product
                {
                    Id = 4,
                    Name = "Silver Earrings",
                    Description = "925 sterling silver earrings",
                    MachineType = MachineType.Earring ,
                    ProductionTime = 5,
                    IsActive = true,
                },

                new Product
                {
                    Id = 5,
                    Name = "Gold Necklace",
                    Description = "24k gold necklace",
                    MachineType = MachineType.Necklace ,
                    ProductionTime = 12,
                    IsActive = true,
                },

                new Product
                {
                    Id = 6,
                    Name = "Gold Ring",
                    Description = "24k gold ring",
                    MachineType = MachineType.Ring ,
                    ProductionTime = 15,
                    IsActive = true,
                },

                new Product
                {
                    Id = 7,
                    Name = "Gold Bracelet",
                    Description = "18k gold bracelet",
                    MachineType = MachineType.Bracelet ,
                    ProductionTime = 8,
                    IsActive = true,
                },

                new Product
                {
                    Id = 8,
                    Name = "Gold Earrings",
                    Description = "24k gold earrings",
                    MachineType = MachineType.Earring ,
                    ProductionTime = 6,
                    IsActive = true,
                },

                new Product
                {
                    Id = 9,
                    Name = "Diamond Necklace",
                    Description = "Gold necklace with diamond details",
                    MachineType = MachineType.Necklace ,
                    ProductionTime = 18,
                    IsActive = true,
                },

                new Product
                {
                    Id = 10,
                    Name = "Diamond Ring",
                    Description = "Gold ring with diamond details",
                    MachineType = MachineType.Ring ,
                    ProductionTime = 20,
                    IsActive = true,
                },

                new Product
                {
                    Id = 11,
                    Name = "Diamond Bracelet",
                    Description = "Gold bracelet with diamond details",
                    MachineType = MachineType.Bracelet ,
                    ProductionTime = 14,
                    IsActive = true,
                },

                new Product
                {
                    Id = 12,
                    Name = "Diamond Earrings",
                    Description = "Gold earrings with diamond details",
                    MachineType = MachineType.Earring ,
                    ProductionTime = 10,
                    IsActive = true,
                }



            );
        }
    }
}