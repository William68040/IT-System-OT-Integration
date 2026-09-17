using Microsoft.EntityFrameworkCore;
using Shared.Data;
using Shared.Enums;

namespace ProductionSystem;

internal class Program
{
    static void Main(string[] args)
    {
        using var db = new FactoryDbContext();

        bool running = true;

        while (running)
        {
            Console.Clear();

            Console.WriteLine("=== PRODUCTION SYSTEM ===");
            Console.WriteLine();
            Console.WriteLine("Waiting production jobs:");
            Console.WriteLine();

            var waitingJobs = db.ProductionJobs
                .Where(j => j.Status == JobStatus.Waiting)
                .Include(j => j.Order)
                .ThenInclude(o => o.Product)
                .ToList();

            if (waitingJobs.Count == 0)
            {
                Console.WriteLine("No production jobs waiting.");
                Pause();
                return;
            }

            foreach (var job in waitingJobs)
            {
                Console.WriteLine(
                    $"Job #{job.Id} - Order #{job.OrderId} - " +
                    $"{job.Order.Product.Name} - " +
                    $"{job.MachineName} - " +
                    $"Quantity: {job.Order.Quantity}");
            }

            Console.WriteLine();
            Console.WriteLine("Enter Job ID to start production.");
            Console.WriteLine("Enter 0 to exit.");
            Console.Write("Choice: ");

            string? input = Console.ReadLine();

            if (!int.TryParse(input, out int jobId))
            {
                Console.WriteLine("Invalid input.");
                Pause();
                continue;
            }

            if (jobId == 0)
            {
                running = false;
                continue;
            }

            StartProduction(db, jobId);
        }
    }

    static void StartProduction(FactoryDbContext db, int jobId)
    {
        var job = db.ProductionJobs
            .Include(j => j.Order)
            .ThenInclude(o => o.Product)
            .FirstOrDefault(j => j.Id == jobId);

        if (job == null)
        {
            Console.WriteLine("Production job not found.");
            Pause();
            return;
        }

        if (job.Status != JobStatus.Waiting)
        {
            Console.WriteLine("This job is not waiting for production.");
            Pause();
            return;
        }

        Console.WriteLine();
        Console.WriteLine($"Selected Job #{job.Id}");
        Console.WriteLine($"Product: {job.Order.Product.Name}");
        Console.WriteLine($"Machine: {job.MachineName}");
        Console.WriteLine($"Quantity: {job.Order.Quantity}");
        Console.WriteLine();
        Console.WriteLine("Ready to start production.");

        Pause();
    }

    static void Pause()
    {
        Console.WriteLine();
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }
}
