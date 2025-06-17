using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// لجنة الصرف
/// </summary>
public partial class Im_PermissionItem_Division_Custody_DismissCommittee
{
    public long ID { get; set; }

    /// <summary>
    /// كود نقل مكان التحفظ
    /// </summary>
    public long Im_PermissionItem_Division_Custody_Id { get; set; }

    /// <summary>
    /// كود اساسيات اللجنه
    /// </summary>
    public long Im_RequestCommittee_Id { get; set; }

    /// <summary>
    /// تاريخ الخروج
    /// </summary>
    public DateOnly? Dismiss_Date { get; set; }

    /// <summary>
    /// وقت الخروج
    /// </summary>
    public TimeOnly? DismissTime { get; set; }

    /// <summary>
    /// 1 if committe accept else 0
    /// </summary>
    public bool IsApproved { get; set; }

    /// <summary>
    /// 1 if investigation is done,0 if not done
    /// if car is come or not
    /// </summary>
    public bool Status { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    /// <summary>
    /// ترصيص
    /// </summary>
    public string? Lock_Lead { get; set; }

    public string? Notes { get; set; }

    public virtual Im_PermissionItem_Division_Custody Im_PermissionItem_Division_Custody { get; set; } = null!;

    public virtual ICollection<Im_PermissionItem_Division_Custody_ReceiveCommittee> Im_PermissionItem_Division_Custody_ReceiveCommittees { get; set; } = new List<Im_PermissionItem_Division_Custody_ReceiveCommittee>();

    public virtual Im_RequestCommittee Im_RequestCommittee { get; set; } = null!;
}
