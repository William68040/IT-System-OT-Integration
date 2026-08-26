using System;
using System.Collections.Generic;
using System.Text;
using Shared.Enums;

namespace Shared.Models;

public class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public MachineType MachineType { get; set; } // vilken maskintyp som behövs för att producera produkten

    public int ProductionTime { get; set; } 

    public bool IsActive { get; set; } = true;

    //navigation property
    public ICollection<Order > Orders { get; set; } = new List<Order>();
}
