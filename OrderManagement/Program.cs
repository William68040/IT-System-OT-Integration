using Microsoft.EntityFrameworkCore;
using Shared.Data;
using Shared.Enums;
using Shared.Models;

namespace OrderManagement
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString =
                "Server=(localdb)\\MSSQLLocalDB;Database=JewelryFactoryDb;Trusted_Connection=True;TrustServerCertificate=True;";

            var options = new DbContextOptionsBuilder<FactoryDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            using var db = new FactoryDbContext(options);

            // Creates the default admin user if it does not already exist
            CreateDefaultUser(db);

            // Login
            if (!Login(db))
            {
                return;
            }

            bool running = true;

            while (running)
            {
                Console.Clear();

                Console.WriteLine("=== JEWELRY FACTORY ===");
                Console.WriteLine();
                Console.WriteLine("1. List Products");
                Console.WriteLine("2. Create Order");
                Console.WriteLine("3. List Orders");
                Console.WriteLine("4. Exit");
                Console.Write("Select an option: ");

                string? input = Console.ReadLine();

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
        }

        static bool Login(FactoryDbContext db)
        {
            Console.Clear();

            Console.WriteLine("=== LOGIN ===");
            Console.WriteLine();

            Console.Write("Username: ");
            string? username = Console.ReadLine();

            Console.Write("Password: ");
            string? password = Console.ReadLine();

            var user = db.Users
                .FirstOrDefault(u => u.Username == username);

            if (user == null)
            {
                Console.WriteLine("Invalid username or password.");
                Pause();
                return false;
            }

            bool passwordCorrect = BCrypt.Net.BCrypt.Verify(
                password ?? "",
                user.PasswordHash);

            if (!passwordCorrect)
            {
                Console.WriteLine("Invalid username or password.");
                Pause();
                return false;
            }

            Console.WriteLine();
            Console.WriteLine($"Login successful. Welcome {user.Username}!");
            Pause();

            return true;
        }

        static void CreateDefaultUser(FactoryDbContext db)
        {
            var existingUser = db.Users
                .FirstOrDefault(u => u.Username == "admin");

            if (existingUser != null)
            {
                return;
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword("Admin123");

            var user = new User
            {
                Username = "admin",
                PasswordHash = passwordHash,
                Role = "Admin"
            };

            db.Users.Add(user);
            db.SaveChanges();
        }

        static void ListProducts(FactoryDbContext db)
        {
            Console.Clear();

            Console.WriteLine("=== PRODUCTS ===");
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
                Console.WriteLine(
                    $"{product.Id}. {product.Name} - {product.Description}");
            }

            Console.WriteLine();

            Console.Write("Choose product ID: ");
            string? productInput = Console.ReadLine();

            if (!int.TryParse(productInput, out int productId))
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

        static void Pause()
        {
            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}