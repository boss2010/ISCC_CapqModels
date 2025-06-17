using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// الموافقة على نتيجة الفحص
/// </summary>
public partial class Station_Accreditation_Committee_CheckList_Confirm
{
    public long ID { get; set; }

    public long Station_Accreditation_CommitteeResult_ID { get; set; }

    public DateTime Date { get; set; }

    public long EmployeeId { get; set; }

    public string? Notes { get; set; }

    public bool IsAccepted { get; set; }

    public virtual Station_Accreditation_Committee_CheckList Station_Accreditation_CommitteeResult { get; set; } = null!;
}
