using Shared.Data;
using Shared.Enums;
using Shared.Models;

namespace IntegrationService;

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
            Console.WriteLine("Pending orders:");
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
            Console.WriteLine("1. Send selected order");
            Console.WriteLine("2. Automatically send all pending orders");
            Console.WriteLine("0. Exit");
            Console.Write("Choice: ");

            string? input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    SendSelectedOrder(db);
                    break;

                case "2":
                    SendAllPendingOrders(db);
                    break;

                case "0":
                    running = false;
                    break;

                default:
                    Console.WriteLine("Invalid choice.");
                    Pause();
                    break;
            }
        }
    }

    static void SendSelectedOrder(FactoryDbContext db)
    {
        Console.Write("Enter Order ID: ");
        string? input = Console.ReadLine();

        if (!int.TryParse(input, out int orderId))
        {
            Console.WriteLine("Invalid Order ID.");
            Pause();
            return;
        }

        SendOrder(db, orderId);

        Pause();
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

    public static void SendAllPendingOrders(FactoryDbContext db)
    {
        var pendingOrders = db.Orders
            .Where(o => o.Status == OrderStatus.Pending)
            .ToList();

        if (pendingOrders.Count == 0)
        {
            Console.WriteLine("No pending orders to send.");
            Pause();
            return;
        }

        Console.WriteLine();
        Console.WriteLine("Automatically processing pending orders...");
        Console.WriteLine();

        foreach (var order in pendingOrders)
        {
            SendOrder(db, order.Id);
        }

        Console.WriteLine();
        Console.WriteLine("All pending orders have been processed.");
        Pause();
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
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
}