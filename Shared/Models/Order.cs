using System;
using System.Collections.Generic;
using System.Text;
using Shared.Enums;

namespace Shared.Models;

public class Order
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public OrderStatus Status { get; set; }

    public Product Product { get; set; } = null!; // navigation property

    public ProductionJob? ProductionJob { get; set; } // navigation property




}
