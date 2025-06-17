using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// طلب الفحص الوارد
/// </summary>
public partial class Im_CheckRequest
{
    public long ID { get; set; }

    public long? Outlet_ID { get; set; }

    /// <summary>
    /// رقم طلب الفحص
    /// </summary>
    public string CheckRequest_Number { get; set; } = null!;

    /// <summary>
    /// الشركة المصدرة
    /// </summary>
    public string? ExportCompany { get; set; }

    /// <summary>
    /// عنوان الشركة المصدرة
    /// </summary>
    public string? ExportCompanyAddress { get; set; }

    public bool? IsPaid { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsAccepted { get; set; }

    public DateTime? IsAccepted_Date { get; set; }

    public short? User_Creation_Id { get; set; }

    public DateTime? User_Creation_Date { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    /// <summary>
    /// المبلغ
    /// </summary>
    public decimal? Amount { get; set; }

    /// <summary>
    /// نوع إذن الاستراد
    /// </summary>
    public byte? Im_OperationType { get; set; }

    public virtual ICollection<Im_CheckRequest_Customs_Message> Im_CheckRequest_Customs_Messages { get; set; } = new List<Im_CheckRequest_Customs_Message>();

    public virtual ICollection<Im_CheckRequest_Datum> Im_CheckRequest_Data { get; set; } = new List<Im_CheckRequest_Datum>();

    public virtual ICollection<Im_CheckRequest_Distribution> Im_CheckRequest_Distributions { get; set; } = new List<Im_CheckRequest_Distribution>();

    public virtual ICollection<Im_CheckRequest_Final_Result> Im_CheckRequest_Final_Results { get; set; } = new List<Im_CheckRequest_Final_Result>();

    public virtual ICollection<Im_CheckRequest_Manafest> Im_CheckRequest_Manafests { get; set; } = new List<Im_CheckRequest_Manafest>();

    public virtual ICollection<Im_CheckRequest_RefuseReason> Im_CheckRequest_RefuseReasons { get; set; } = new List<Im_CheckRequest_RefuseReason>();

    public virtual ICollection<Im_CheckRequest_Visa> Im_CheckRequest_Visas { get; set; } = new List<Im_CheckRequest_Visa>();

    public virtual ICollection<Im_CheckRequset_Shipping_Method> Im_CheckRequset_Shipping_Methods { get; set; } = new List<Im_CheckRequset_Shipping_Method>();

    public virtual ICollection<Im_CustodyPlace_CheckRequest> Im_CustodyPlace_CheckRequests { get; set; } = new List<Im_CustodyPlace_CheckRequest>();

    public virtual ICollection<Im_PermissionRequest> Im_PermissionRequests { get; set; } = new List<Im_PermissionRequest>();

    public virtual ICollection<Im_RequestCommittee> Im_RequestCommittees { get; set; } = new List<Im_RequestCommittee>();

    public virtual Outlet? Outlet { get; set; }
}
