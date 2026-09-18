using Shared.Data;
using Shared.Enums;
using Shared.Models;

namespace IntegrationService
{
    public class Program
    {
        static void Main(string[] args)
        {
            using var db = new FactoryDbContext();

            bool running = true;

            while (running)
            {
                Console.Clear();

                Console.WriteLine("=== INTEGRATION SERVICE ===");
                Console.WriteLine();
                Console.WriteLine("Orders waiting for integration:");
                Console.WriteLine();

                var pendingOrders = db.Orders
                    .Where(o => o.Status == OrderStatus.Pending)
                    .ToList();

                if (pendingOrders.Count == 0)
                {
                    Console.WriteLine("No pending orders.");
                    Pause();
                    return;
                }

                foreach (var order in pendingOrders)
                {
                    var product = db.Products.Find(order.ProductId);

                    if (product != null)
                    {
                        Console.WriteLine(
                            $"Order #{order.Id} - {product.Name} - Quantity: {order.Quantity}");
                    }
                }

                Console.WriteLine();
                Console.WriteLine("Enter Order ID to send.");
                Console.WriteLine("Enter 0 to exit.");
                Console.Write("Choice: ");

                string? input = Console.ReadLine();

                if (!int.TryParse(input, out int orderId))
                {
                    Console.WriteLine("Invalid input.");
                    Pause();
                    continue;
                }

                if (orderId == 0)
                {
                    running = false;
                    continue;
                }

                SendOrder(db, orderId);
            }
        }

        public static void SendOrder(FactoryDbContext db, int orderId)
        {
            var order = db.Orders
                .FirstOrDefault(o => o.Id == orderId);

            if (order == null)
            {
                Console.WriteLine("Order not found.");
                
                return;
            }

            if (order.Status != OrderStatus.Pending)
            {
                Console.WriteLine("This order is not waiting for integration.");
                
                return;
            }

            var product = db.Products
                .FirstOrDefault(p => p.Id == order.ProductId);

            if (product == null)
            {
                Console.WriteLine("Product not found.");
                
                return;
            }

            string machineName = GetMachineName(product.MachineType);

            var productionJob = new ProductionJob
            {
                OrderId = order.Id,
                MachineName = machineName,
                Status = JobStatus.Waiting,
                CreatedAt = DateTime.Now
            };

            db.ProductionJobs.Add(productionJob);

            order.Status = OrderStatus.Sent;

            db.SaveChanges();

            Console.WriteLine();
            Console.WriteLine($"Order #{order.Id} sent successfully.");
            Console.WriteLine($"Product: {product.Name}");
            Console.WriteLine($"Machine: {machineName}");
            Console.WriteLine("Production job created.");
            Console.WriteLine("Order status changed to Sent.");

            
        }

        static string GetMachineName(MachineType machineType)
        {
            switch (machineType)
            {
                case MachineType.Necklace:
                    return "Necklace Machine";

                case MachineType.Bracelet:
                    return "Bracelet Machine";

                case MachineType.Ring:
                    return "Ring Machine";

                case MachineType.Earring:
                    return "Earring Machine";

                default:
                    return "Unknown Machine";
            }

        }
        static void Pause()
        {
            Console.WriteLine();
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }
    }
}
