using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Websitetype
{
    public int ID { get; set; }

    public string? TypeAr { get; set; }

    public string? TypeEN { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsImage { get; set; }
}
