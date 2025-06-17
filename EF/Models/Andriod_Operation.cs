using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Andriod_Operation
{
    public byte Id { get; set; }

    public string Operation_Name { get; set; } = null!;

    public virtual ICollection<Andriod_Location> Andriod_Locations { get; set; } = new List<Andriod_Location>();
}
