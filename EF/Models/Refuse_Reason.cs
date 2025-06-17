using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// اسباب رفض الطلب( صادر/وارد)
/// </summary>
public partial class Refuse_Reason
{
    public short ID { get; set; }

    public string? Name_Ar { get; set; }

    public string? Name_En { get; set; }

    public bool IsActive { get; set; }

    public bool? IsStop { get; set; }

    public long User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public long? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public long? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    /// <summary>
    /// هل يرفض الطلب كله في حالة وجود إصابة
    /// </summary>
    public int? IsExport { get; set; }

    public int? Refused_stopped { get; set; }

    /// <summary>
    /// from systemcode table 20
    ///  اذن استراد 
    ///  طلب فحص وارد 74
    /// farm 78
    /// 
    /// </summary>
    public int? A_SystemCode_ID { get; set; }

    public virtual ICollection<Ex_CheckRequest_RefuseReason> Ex_CheckRequest_RefuseReasons { get; set; } = new List<Ex_CheckRequest_RefuseReason>();

    public virtual ICollection<Farm_Request_Refuse_Reason> Farm_Request_Refuse_Reasons { get; set; } = new List<Farm_Request_Refuse_Reason>();

    public virtual ICollection<Im_CheckRequest_RefuseReason> Im_CheckRequest_RefuseReasons { get; set; } = new List<Im_CheckRequest_RefuseReason>();

    public virtual ICollection<Im_PermissionRequest_RefuseReason> Im_PermissionRequest_RefuseReasons { get; set; } = new List<Im_PermissionRequest_RefuseReason>();
}
