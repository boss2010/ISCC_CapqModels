using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Farm_Committee_CheckList_Confirm
{
    public long ID { get; set; }

    public long Farm_Committee_CheckList_ID { get; set; }

    public DateTime Date { get; set; }

    public long EmployeeId { get; set; }

    public string? Notes { get; set; }

    public bool IsAccepted { get; set; }

    public virtual Farm_Committee_CheckList Farm_Committee_CheckList { get; set; } = null!;
}
