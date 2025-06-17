using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Ex_CommitteeCheckLocation
{
    public byte ID { get; set; }

    public string? Name_Ar { get; set; }

    public string? Name_En { get; set; }

    public virtual ICollection<Ex_RequestCommittee> Ex_RequestCommittees { get; set; } = new List<Ex_RequestCommittee>();
}
