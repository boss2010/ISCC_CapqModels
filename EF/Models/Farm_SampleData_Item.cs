using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Farm_SampleData_Item
{
    public long ID { get; set; }

    /// <summary>
    /// لجنة المعالجة
    /// </summary>
    public long FarmCommittee_ID { get; set; }

    public long? Farm_Request_ItemCategories_ID { get; set; }

    public int AnalysisLabType_ID { get; set; }

    /// <summary>
    /// تاريخ سحب العينة
    /// </summary>
    public DateOnly? WithdrawDate { get; set; }

    /// <summary>
    /// البار كود
    /// </summary>
    public string? Sample_BarCode { get; set; }

    /// <summary>
    /// حجم العينة
    /// </summary>
    public double? SampleSize { get; set; }

    /// <summary>
    /// نسبة اخذ العينة
    /// </summary>
    public double? SampleRatio { get; set; }

    /// <summary>
    /// 0 if rejected else 1 
    /// موافقه المعمل
    /// </summary>
    public bool? IsAccepted { get; set; }

    /// <summary>
    /// ملاحظات الاندريد
    /// </summary>
    public string? Notes_Ar { get; set; }

    /// <summary>
    /// سبب الرفض للمعمل ar
    /// </summary>
    public string? RejectReason_Ar { get; set; }

    /// <summary>
    /// سبب الرفض للمعمل en
    /// </summary>
    public string? RejectReason_En { get; set; }

    public string? Notes_En { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    /// <summary>
    /// موقف الحجر
    /// </summary>
    public bool? Admin_Confirmation { get; set; }

    /// <summary>
    /// ادمن الحجر
    /// </summary>
    public short? Admin_User { get; set; }

    public DateTime? Admin_Date { get; set; }

    public bool? IsPrint { get; set; }

    public virtual AnalysisLabType AnalysisLabType { get; set; } = null!;

    public virtual Farm_Committee FarmCommittee { get; set; } = null!;

    public virtual Farm_Request_ItemCategory? Farm_Request_ItemCategories { get; set; }

    public virtual ICollection<Farm_SampleData_Confirm_Item> Farm_SampleData_Confirm_Items { get; set; } = new List<Farm_SampleData_Confirm_Item>();
}
