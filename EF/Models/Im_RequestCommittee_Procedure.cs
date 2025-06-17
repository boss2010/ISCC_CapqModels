using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// اجراءات اللجنة
/// </summary>
public partial class Im_RequestCommittee_Procedure
{
    public long ID { get; set; }

    public long Im_RequestCommittee_ID { get; set; }

    /// <summary>
    /// إجراءات تتم على اللوط (نقل تحت تحفظ/فحص/تحاليل/...)
    /// </summary>
    public byte Im_ProcedureType_ID { get; set; }

    /// <summary>
    /// السبب
    /// </summary>
    public string? Reason_Text { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public virtual Im_ProcedureType Im_ProcedureType { get; set; } = null!;

    public virtual Im_RequestCommittee Im_RequestCommittee { get; set; } = null!;
}
