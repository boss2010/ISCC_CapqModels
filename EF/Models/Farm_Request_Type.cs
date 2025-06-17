using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Farm_Request_Type
{
    public byte ID { get; set; }

    public string? Name_AR { get; set; }

    public string? Name_EN { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<Farm_Request> Farm_Requests { get; set; } = new List<Farm_Request>();
}
