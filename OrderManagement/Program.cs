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

            Console.WriteLine("Database connection configured.");


        }
    }
}
