using System;
using System.Collections.Generic;

namespace EF.Imports.Entities;

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

    public string? Completion_Notes { get; set; }

    public byte? Completion_Status { get; set; }

    public DateTime? Completion_Request_Date { get; set; }

    public DateTime? Completion_Response_Date { get; set; }

    public short? Completion_Response_User_Id { get; set; }

    public virtual ICollection<Im_CheckRequest_Data> Im_CheckRequest_Data { get; set; } = new List<Im_CheckRequest_Data>();

    public virtual ICollection<Im_CheckRequset_Shipping_Method> Im_CheckRequset_Shipping_Method { get; set; } = new List<Im_CheckRequset_Shipping_Method>();

    public virtual ICollection<Im_RequestCommittee> Im_RequestCommittee { get; set; } = new List<Im_RequestCommittee>();

    public virtual Outlet? Outlet { get; set; }
}
