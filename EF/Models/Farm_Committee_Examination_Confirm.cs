using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// الموافقة على نتيجة الفحص
/// </summary>
public partial class Farm_Committee_Examination_Confirm
{
    public long ID { get; set; }

    public long Farm_Committee_Exmination_ID { get; set; }

    public DateTime Date { get; set; }

    public short EmployeeId { get; set; }

    public string? Notes { get; set; }

    public bool IsAccepted { get; set; }

    public virtual Farm_Committee_Examination Farm_Committee_Exmination { get; set; } = null!;
}
