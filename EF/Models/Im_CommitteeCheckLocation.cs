using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// أماكن الفحص التي يحددها المنفذ
/// </summary>
public partial class Im_CommitteeCheckLocation
{
    public byte ID { get; set; }

    public string? Name_Ar { get; set; }

    public string? Name_En { get; set; }

    public virtual ICollection<Im_RequestCommittee> Im_RequestCommittees { get; set; } = new List<Im_RequestCommittee>();
}
