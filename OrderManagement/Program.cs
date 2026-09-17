using Microsoft.EntityFrameworkCore;

using Shared.Data;

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
                        Console.WriteLine("Create order - not implemented yet.");
                        Pause();
                        break;

                    case "3":
                        Console.WriteLine("List orders - not implemented yet.");
                        Pause();
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



        }
    }
}
