using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// إذن استيراد
/// </summary>
public partial class Im_PermissionRequest
{
    public long ID { get; set; }

    public decimal? ImPermission_Number { get; set; }

    /// <summary>
    /// تاريخ وصول الشحنة
    /// </summary>
    public DateOnly Arrival_Date { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short? User_Creation_Id { get; set; }

    public DateTime? User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public bool? IsPaid { get; set; }

    public bool? IsAcceppted { get; set; }

    public bool? IS_Print_Ar { get; set; }

    public bool? IS_Print_EN { get; set; }

    public bool? IS_Notice_Arrival { get; set; }

    public long? Im_CheckRequest_ID { get; set; }

    /// <summary>
    /// حاله التجديد
    /// </summary>
    public byte? Renewal_Status { get; set; }

    /// <summary>
    /// عدد مرات التجديد
    /// </summary>
    public byte? Print_Count { get; set; }

    /// <summary>
    /// تاريخ الاصدار
    /// </summary>
    public DateOnly? Start_Date { get; set; }

    /// <summary>
    /// تاريخ اخر طباعه
    /// </summary>
    public DateOnly? End_Date { get; set; }

    /// <summary>
    /// المبلغ
    /// </summary>
    public decimal? Amount { get; set; }

    public virtual Im_CheckRequest? Im_CheckRequest { get; set; }

    public virtual ICollection<Im_PermissionRequest_RefuseReason> Im_PermissionRequest_RefuseReasons { get; set; } = new List<Im_PermissionRequest_RefuseReason>();

    public virtual ICollection<Im_ScientificResearch> Im_ScientificResearches { get; set; } = new List<Im_ScientificResearch>();
}
