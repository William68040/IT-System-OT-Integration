using Microsoft.EntityFrameworkCore;
using Shared.Data;
using Shared.Enums;
using Shared.Models;
using System.Net.NetworkInformation;

namespace OrderManagement
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // uppgift Integrerar ett simulerat IT-system (t.ex. ett enkelt ärendehanteringssystem eller lagersystem) med ett 
            // simulerat OT-system (t.ex en simulerad produktionslinje som generar sensorvärden eller händelser med tre program.
            // simulerad orderhantering, simulerad OT system och integrationsprogram (som t.ex läser orderdatabas och skickar till OT med modbus tcp/ip protokoll).

            string connectionString =
                          "Server=(localdb)\\MSSQLLocalDB;Database=JewelryFactoryDb;Trusted_Connection=True;TrustServerCertificate=True;";

            var options = new DbContextOptionsBuilder<FactoryDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            using var db = new FactoryDbContext(options);

            bool running = true;

            while (running)
            {
                Console.Clear();

                Console.WriteLine("=== Jewelry Factory ===");
                Console.WriteLine();
                Console.WriteLine("1. List Products");
                Console.WriteLine("2. Create Orders");
                Console.WriteLine("3. List Orders");
                Console.WriteLine("4. Exit");
                Console.Write("Select an option: ");
                var input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        ListProducts(db);
                        break;

                    case "2":
                        CreateOrder(db);
                        break;

                    case "3":
                        ListOrders(db);
                        break;

                    case "4":
                        running = false;
                        break;

                    default:
                        Console.WriteLine("Invalid choice.");
                        Pause();
                        break;
                }
            }


            static void Pause()
            {
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();  
            }

            static void ListProducts(FactoryDbContext db)
            { 
                Console.Clear();

                Console.WriteLine("=== Products ===");
                Console.WriteLine();

                var products = db.Products.ToList();

                foreach (var product in products)
                {
                    Console.WriteLine($"ID: {product.Id}");
                    Console.WriteLine($"Name: {product.Name}");
                    Console.WriteLine($"Description: {product.Description}");
                    Console.WriteLine($"Machine: {product.MachineType}");
                    Console.WriteLine($"Production time: {product.ProductionTime} minutes");
                    Console.WriteLine($"Active: {product.IsActive}");
                    Console.WriteLine("----------------------------");
                }

                Pause();

            }

            static void CreateOrder(FactoryDbContext db)
            {
                Console.Clear();

                Console.WriteLine("=== CREATE ORDER ===");
                Console.WriteLine();

                var products = db.Products.ToList();

                foreach (var product in products)
                {
                    Console.WriteLine($"{product.Id}. {product.Name} - {product.Description}");
                }

                Console.WriteLine();

                Console.Write("Choose product ID: ");
                string? productInput = Console.ReadLine();

                if (!int.TryParse(productInput, out int productId)) // checks to see if the input is a valid integer
                {
                    Console.WriteLine("Invalid product ID.");
                    Pause();
                    return;
                }

                var productToOrder = db.Products.Find(productId);

                if (productToOrder == null)
                {
                    Console.WriteLine("Product not found.");
                    Pause();
                    return;
                }

                Console.Write("Enter quantity: ");
                string? quantityInput = Console.ReadLine();

                if (!int.TryParse(quantityInput, out int quantity) || quantity <= 0)
                {
                    Console.WriteLine("Quantity must be greater than 0.");
                    Pause();
                    return;
                }

                var order = new Order
                {
                    ProductId = productToOrder.Id,
                    Quantity = quantity,
                    Status = OrderStatus.Pending
                };

                db.Orders.Add(order);
                db.SaveChanges();

                Console.WriteLine();
                Console.WriteLine("Order created successfully!");
                Console.WriteLine($"Product: {productToOrder.Name}");
                Console.WriteLine($"Quantity: {quantity}");
                Console.WriteLine($"Status: {order.Status}");

                Pause();
            }

            static void ListOrders(FactoryDbContext db)
            {
                Console.Clear();

                Console.WriteLine("=== ORDERS ===");
                Console.WriteLine();

                var orders = db.Orders
                    .Include(o => o.Product)
                    .ToList();

                if (orders.Count == 0)
                {
                    Console.WriteLine("No orders found.");
                    Pause();
                    return;
                }

                foreach (var order in orders)
                {
                    Console.WriteLine($"Order ID: {order.Id}");
                    Console.WriteLine($"Product: {order.Product.Name}");
                    Console.WriteLine($"Quantity: {order.Quantity}");
                    Console.WriteLine($"Status: {order.Status}");
                    Console.WriteLine($"Created: {order.CreatedAt}");
                    Console.WriteLine("----------------------------");
                }

                Pause();
            }

        }
    }
}
