using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// لجنة اعتماد المحطات
/// </summary>
public partial class Station_Accreditation_Committee
{
    public long ID { get; set; }

    /// <summary>
    /// طلب الفحص
    /// </summary>
    public long Station_Accreditation_Request_ID { get; set; }

    public byte CommitteeType_ID { get; set; }

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
    /// null-&gt;ask for accredation
    /// 0-&gt;not accepted
    /// 1-&gt;Accepted
    /// </summary>
    public bool? IsApproved { get; set; }

    /// <summary>
    /// تم الانتهاء من الدفع
    /// </summary>
    public bool? IsPaid { get; set; }

    /// <summary>
    /// المبلغ
    /// </summary>
    public decimal Amount_Total { get; set; }

    /// <summary>
    /// null-&gt;No committe ,0 if not done, 1 if investigation is done
    /// </summary>
    public bool? Status { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public string? Notes_Refuse_En { get; set; }

    public string? Notes_Refuse_Ar { get; set; }

    /// <summary>
    /// 0 if rejected else 1 
    /// lab will set the result
    /// </summary>
    public bool? IsAccepted { get; set; }

    /// <summary>
    /// تعزر عمل اللجنه
    /// </summary>
    public bool? Is_Start_Android { get; set; }

    /// <summary>
    /// تعزر عمل اللجنه
    /// </summary>
    public bool? Is_Cancel { get; set; }

    public virtual CommitteeType CommitteeType { get; set; } = null!;

    public virtual ICollection<Station_Accreditation_Committee_CheckList> Station_Accreditation_Committee_CheckLists { get; set; } = new List<Station_Accreditation_Committee_CheckList>();

    public virtual ICollection<Station_Accreditation_Committee_Final_Result> Station_Accreditation_Committee_Final_Results { get; set; } = new List<Station_Accreditation_Committee_Final_Result>();

    public virtual ICollection<Station_Accreditation_Committee_Imge> Station_Accreditation_Committee_Imges { get; set; } = new List<Station_Accreditation_Committee_Imge>();

    public virtual ICollection<Station_Accreditation_Committee_Shift> Station_Accreditation_Committee_Shifts { get; set; } = new List<Station_Accreditation_Committee_Shift>();

    public virtual Station_Accreditation_Request Station_Accreditation_Request { get; set; } = null!;

    public virtual ICollection<Station_Accreditation_Request_Fees_ENG> Station_Accreditation_Request_Fees_ENGs { get; set; } = new List<Station_Accreditation_Request_Fees_ENG>();
}
