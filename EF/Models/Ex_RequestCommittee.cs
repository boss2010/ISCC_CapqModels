using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Ex_RequestCommittee
{
    public long ID { get; set; }

    public long? ExCheckRequest_ID { get; set; }

    public byte? CommitteeType_ID { get; set; }

    public byte? ExCommitteeCheckLocation_ID { get; set; }

    public DateOnly? Delegation_Date { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public bool? IsFinishedAll { get; set; }

    public bool? IsApproved { get; set; }

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

    public virtual Ex_CheckRequest? ExCheckRequest { get; set; }

    public virtual Ex_CommitteeCheckLocation? ExCommitteeCheckLocation { get; set; }

    public virtual ICollection<Ex_CheckRequest_SampleDatum> Ex_CheckRequest_SampleData { get; set; } = new List<Ex_CheckRequest_SampleDatum>();

    public virtual ICollection<Ex_CommitteeResult> Ex_CommitteeResults { get; set; } = new List<Ex_CommitteeResult>();

    public virtual ICollection<Ex_RequestCommittee_Fees_ENG> Ex_RequestCommittee_Fees_ENGs { get; set; } = new List<Ex_RequestCommittee_Fees_ENG>();

    public virtual ICollection<Ex_RequestCommittee_Shift> Ex_RequestCommittee_Shifts { get; set; } = new List<Ex_RequestCommittee_Shift>();

    public virtual ICollection<Ex_Request_TreatmentDatum> Ex_Request_TreatmentData { get; set; } = new List<Ex_Request_TreatmentDatum>();
}
