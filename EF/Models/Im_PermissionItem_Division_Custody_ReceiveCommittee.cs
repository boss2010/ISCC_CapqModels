using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// لجنة الاستلام
/// </summary>
public partial class Im_PermissionItem_Division_Custody_ReceiveCommittee
{
    public long ID { get; set; }

    /// <summary>
    /// كود لجنه الصرف
    /// </summary>
    public long Im_PermissionItem_Division_Custody_DismissCommittee_Id { get; set; }

    /// <summary>
    /// تاريخ الاستلام
    /// </summary>
    public DateOnly? Receive_Date { get; set; }

    /// <summary>
    /// وقت الاستلام
    /// </summary>
    public TimeOnly? ReceiveTime { get; set; }

    /// <summary>
    /// 0 if exporter doesn&apos;t accept else 1
    /// </summary>
    public bool IsApproved { get; set; }

    /// <summary>
    /// 0 if not done, 1 if investigation is done
    /// </summary>
    public bool Status { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    /// <summary>
    /// الوزن الاجمالي
    /// </summary>
    public decimal GrossWeight { get; set; }

    public string? Notes { get; set; }

    /// <summary>
    /// كود اساسيات اللجنه
    /// </summary>
    public long Im_RequestCommittee_Id { get; set; }

    public virtual Im_PermissionItem_Division_Custody_DismissCommittee Im_PermissionItem_Division_Custody_DismissCommittee { get; set; } = null!;

    public virtual Im_RequestCommittee Im_RequestCommittee { get; set; } = null!;
}
