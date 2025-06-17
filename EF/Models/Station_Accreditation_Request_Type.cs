using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Station_Accreditation_Request_Type
{
    public byte ID { get; set; }

    public string? Name_AR { get; set; }

    public string? Name_EN { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<Station_Accreditation_Request> Station_Accreditation_Requests { get; set; } = new List<Station_Accreditation_Request>();
}
