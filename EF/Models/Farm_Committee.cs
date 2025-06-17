using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// تشكيل لجنة مزرعة
/// </summary>
public partial class Farm_Committee
{
    public long ID { get; set; }

    /// <summary>
    /// طلب الفحص
    /// </summary>
    public long Farm_Request_ID { get; set; }

    public byte? CommitteeType_ID { get; set; }

    /// <summary>
    /// عدد السحبات
    /// </summary>
    public byte? analysis_count { get; set; }

    /// <summary>
    /// تاريخ الفحص-تاريخ الانتداب
    /// </summary>
    public DateOnly? Delegation_Date { get; set; }

    /// <summary>
    ///  بداية ساعة الفحص 
    /// </summary>
    public TimeOnly? StartTime { get; set; }

    /// <summary>
    /// انتهاء ساعة الفحص
    /// </summary>
    public TimeOnly? EndTime { get; set; }

    /// <summary>
    /// null-&gt;exporter not take action 
    /// 0 if exporter doesn&apos;t accept else 1
    /// 
    /// null تم طلب لجنة
    /// 0 تم رفض الطلب من العميل
    /// 1 تم قبول الطلب من العميل
    /// 
    /// </summary>
    public bool? IsApproved { get; set; }

    /// <summary>
    /// null-&gt;No committe ,0 if not done, 1 if investigation is done
    /// 
    /// null لم يتم تشكيل اللجنة
    /// 0 تم التشكيل ولم يتم خروج اللجنة
    /// 1 انتهاء عمل اللجنة
    /// 
    /// </summary>
    public bool? Status { get; set; }

    /// <summary>
    /// 0 if exporter doesn&apos;t accept else 1  خاص ب شغل موظف الحجر في فحص الشحنه
    /// </summary>
    public bool? IsFinishedAll { get; set; }

    /// <summary>
    /// المبلغ
    /// </summary>
    public decimal Amount_Total { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    /// <summary>
    /// تم الانتهاء من الدفع
    /// </summary>
    public bool? IsPaid { get; set; }

    public byte? ShiftTiming_ID { get; set; }

    /// <summary>
    /// تعزر عمل اللجنه
    /// </summary>
    public bool? Is_Cancel { get; set; }

    /// <summary>
    /// اسباب الرفض
    /// </summary>
    public string? Refuse_Reason_Nots { get; set; }

    /// <summary>
    /// تعزر عمل اللجنه
    /// </summary>
    public bool? Is_Start_Android { get; set; }

    public virtual CommitteeType? CommitteeType { get; set; }

    public virtual ICollection<Farm_Committee_CheckList> Farm_Committee_CheckLists { get; set; } = new List<Farm_Committee_CheckList>();

    public virtual ICollection<Farm_Committee_Constrain> Farm_Committee_Constrains { get; set; } = new List<Farm_Committee_Constrain>();

    public virtual ICollection<Farm_Committee_Examination> Farm_Committee_Examinations { get; set; } = new List<Farm_Committee_Examination>();

    public virtual ICollection<Farm_Committee_Final_Result> Farm_Committee_Final_Results { get; set; } = new List<Farm_Committee_Final_Result>();

    public virtual ICollection<Farm_Committee_Shift> Farm_Committee_Shifts { get; set; } = new List<Farm_Committee_Shift>();

    public virtual Farm_Request Farm_Request { get; set; } = null!;

    public virtual ICollection<Farm_SampleDatum> Farm_SampleData { get; set; } = new List<Farm_SampleDatum>();

    public virtual ICollection<Farm_SampleData_Item> Farm_SampleData_Items { get; set; } = new List<Farm_SampleData_Item>();
}
