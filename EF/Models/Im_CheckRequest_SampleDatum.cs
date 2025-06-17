using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// نتيجه سحب عينه للوارد (admin)
/// </summary>
public partial class Im_CheckRequest_SampleDatum
{
    public long ID { get; set; }

    public int AnalysisLabType_ID { get; set; }

    /// <summary>
    /// كود اساسيات اللجنه
    /// </summary>
    public long Im_RequestCommittee_ID { get; set; }

    public long Im_Request_Item_Id { get; set; }

    /// <summary>
    /// null for the whole request  /  كود بيانات الدفعه
    /// </summary>
    public long? LotData_ID { get; set; }

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
    /// 
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

    /// <summary>
    /// (0) in the case of all,(1) in the case of the part   في حاله الجزئي او الكلي
    /// </summary>
    public bool? IS_Total { get; set; }

    /// <summary>
    /// الاسم المختصر 
    /// </summary>
    public long? Item_ShortName_ID { get; set; }

    /// <summary>
    /// في حاله الفحص لو كلي واتحول الي جزئي
    /// </summary>
    public bool? IS_Total_Android { get; set; }

    /// <summary>
    /// مين رمي row (system or android)
    /// </summary>
    public bool? IS_From_Android { get; set; }

    /// <summary>
    /// رقم الختامه والسيل الملاحي
    /// </summary>
    public string? Syl_ALkhatima_Number { get; set; }

    /// <summary>
    /// المبلغ
    /// </summary>
    public decimal? Amount { get; set; }

    public decimal? Fees_Actual { get; set; }

    public bool? IsPaid { get; set; }

    public virtual AnalysisLabType AnalysisLabType { get; set; } = null!;

    public virtual ICollection<Im_CheckRequest_SampleData_Confirm> Im_CheckRequest_SampleData_Confirms { get; set; } = new List<Im_CheckRequest_SampleData_Confirm>();

    public virtual Im_RequestCommittee Im_RequestCommittee { get; set; } = null!;
}
