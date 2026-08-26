using System;
using System.Collections.Generic;
using System.Text;
using Shared.Enums;

namespace Shared.Models;

public class ProductionJob
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public string MachineName { get; set; } = string.Empty;

    public JobStatus Status { get; set; }
    
    public DateTime CreatedAt { get; set; }

    public DateTime? StartedAt { get; set; }

    public DateTime? FinishedAt { get; set; }


    public Order Order { get; set; } = null!; // navigation property

}
