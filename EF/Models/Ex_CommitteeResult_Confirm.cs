using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Ex_CommitteeResult_Confirm
{
    public long ID { get; set; }

    public long Ex_CommitteeResult_ID { get; set; }

    public DateTime Date { get; set; }

    public long EmployeeId { get; set; }

    public string? Notes { get; set; }

    public bool IsAccepted { get; set; }

    public virtual Ex_CommitteeResult Ex_CommitteeResult { get; set; } = null!;
}
