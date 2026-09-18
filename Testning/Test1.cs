using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shared.Data;
using Shared.Enums;
using Shared.Models;
using IntegrationService;
using ProductionSystem;

namespace FactoryIntegration.Tests;

[TestClass]
public class FactoryTests
{
    [TestMethod]
    public void SendOrder_ShouldCreateProductionJobAndSetOrderToSent()
    {
        using var db = new FactoryDbContext();

        // Create a test order
        var order = new Order
        {
            ProductId = 1,
            Quantity = 1,
            Status = OrderStatus.Pending
        };

        db.Orders.Add(order);
        db.SaveChanges();

        try
        {
            // Run integration
            IntegrationService.Program.SendOrder(db, order.Id);
                
            var updatedOrder = db.Orders
                .First(o => o.Id == order.Id);

            // Check that the order was sent
            Assert.AreEqual(OrderStatus.Sent, updatedOrder.Status);

            // Check that a production job was created
            var productionJob = db.ProductionJobs
                .FirstOrDefault(j => j.OrderId == order.Id);

            Assert.IsNotNull(productionJob);
            Assert.AreEqual(JobStatus.Waiting, productionJob.Status);
        }
        finally
        {
            
            var productionJob = db.ProductionJobs
                .FirstOrDefault(j => j.OrderId == order.Id);

            if (productionJob != null)
            {
                db.ProductionJobs.Remove(productionJob);
            }

            db.Orders.Remove(order);
            db.SaveChanges();
        }
    }

    [TestMethod]
    public void StartProduction_ShouldFinishJobAndCompleteOrder()
    {
        using var db = new FactoryDbContext();

        // Product 4 = Silver Earring
        // Production time = 5 minutes
        var order = new Order
        {
            ProductId = 4,
            Quantity = 1,
            Status = OrderStatus.Sent
        };

        db.Orders.Add(order);
        db.SaveChanges();

        var productionJob = new ProductionJob
        {
            OrderId = order.Id,
            MachineName = "Earring Machine",
            Status = JobStatus.Waiting,
            CreatedAt = DateTime.Now
        };

        db.ProductionJobs.Add(productionJob);
        db.SaveChanges();

        try
        {
            // Run production
            ProductionSystem.Program.StartProduction(db, productionJob.Id);

            var updatedJob = db.ProductionJobs
                .First(j => j.Id == productionJob.Id);

            var updatedOrder = db.Orders
                .First(o => o.Id == order.Id);

            // Check that production finished
            Assert.AreEqual(JobStatus.Finished, updatedJob.Status);

            // Check that the order is completed
            Assert.AreEqual(OrderStatus.Completed, updatedOrder.Status);

            
            Assert.IsNotNull(updatedJob.StartedAt);
            Assert.IsNotNull(updatedJob.FinishedAt);
        }
        finally
        {
            db.ProductionJobs.Remove(productionJob);
            db.Orders.Remove(order);
            db.SaveChanges();
        }
    }
}