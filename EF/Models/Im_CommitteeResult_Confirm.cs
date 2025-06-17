using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// راي المساعد
/// </summary>
public partial class Im_CommitteeResult_Confirm
{
    public long ID { get; set; }

    public long Im_CommitteeResult_ID { get; set; }

    public DateTime Date { get; set; }

    public long EmployeeId { get; set; }

    public string? Notes { get; set; }

    public bool IsAccepted { get; set; }

    public virtual Im_CommitteeResult Im_CommitteeResult { get; set; } = null!;
}
