using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// نبطشيات اللجنه
/// </summary>
public partial class Im_RequestCommittee_Shift
{
    public long ID { get; set; }

    public long Im_RequestCommittee_ID { get; set; }

    public byte ShiftTiming_ID { get; set; }

    public byte? Count { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    /// <summary>
    /// المبلغ
    /// </summary>
    public decimal? Amount { get; set; }

    public bool? IsPaid { get; set; }

    public virtual Im_RequestCommittee Im_RequestCommittee { get; set; } = null!;

    public virtual ShiftTiming ShiftTiming { get; set; } = null!;
}
