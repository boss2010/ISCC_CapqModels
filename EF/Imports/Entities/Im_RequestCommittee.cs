using System;
using System.Collections.Generic;

namespace EF.Imports.Entities;

/// <summary>
/// اساسيات اللجان
/// </summary>
public partial class Im_RequestCommittee
{
    public long ID { get; set; }

    public long? ImCheckRequest_ID { get; set; }

    /// <summary>
    /// كود الغرض من اللجنه
    /// </summary>
    public byte? CommitteeType_ID { get; set; }

    /// <summary>
    /// كود اماكن الفحص
    /// </summary>
    public byte? ImCommitteeCheckLocation_ID { get; set; }

    /// <summary>
    /// تاريخ الانتداب
    /// </summary>
    public DateOnly? Delegation_Date { get; set; }

    /// <summary>
    ///  بداية ساعة الفحص 
    /// </summary>
    public TimeOnly StartTime { get; set; }

    /// <summary>
    /// انتهاء ساعة الفحص
    /// </summary>
    public TimeOnly EndTime { get; set; }

    /// <summary>
    /// 0 if exporter doesn&apos;t accept else 1 خاص ب شغل موظف الحجر في فحص الشحنه
    /// </summary>
    public bool? IsFinishedAll { get; set; }

    /// <summary>
    /// 0 if exporter doesn&apos;t accept else 1 خاص ب نتيجه الفحص
    /// </summary>
    public bool? IsApproved { get; set; }

    /// <summary>
    /// 0 if not done, 1 if investigation is done تم نزول اللجنه 1 ,0 وافق علي معاد اللجنه , null خاص ب العميل عدم الرد عل معاد اللجنه , 
    /// </summary>
    public bool? Status { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public bool? IsAgreeResult { get; set; }

    /// <summary>
    /// تم الانتهاء من الدفع
    /// </summary>
    public bool? IsPaid { get; set; }

    /// <summary>
    /// تعزر عمل اللجنه
    /// </summary>
    public bool? Is_Start_Android { get; set; }

    /// <summary>
    /// لايقاف او حذف اللجنة مربوط مع  A_SystemCode رقم 31
    /// 
    /// </summary>
    public byte? Is_Cancel { get; set; }

    public virtual CommitteeType? CommitteeType { get; set; }

    public virtual Im_CheckRequest? ImCheckRequest { get; set; }

    public virtual ICollection<Im_CheckRequest_SampleData> Im_CheckRequest_SampleData { get; set; } = new List<Im_CheckRequest_SampleData>();

    public virtual ICollection<Im_CommitteeResult> Im_CommitteeResult { get; set; } = new List<Im_CommitteeResult>();

    public virtual ICollection<Im_Request_TreatmentData> Im_Request_TreatmentData { get; set; } = new List<Im_Request_TreatmentData>();
}
