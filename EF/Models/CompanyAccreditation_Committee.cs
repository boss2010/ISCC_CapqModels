using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class CompanyAccreditation_Committee
{
    public long ID { get; set; }

    /// <summary>
    /// طلب الفحص
    /// </summary>
    public long Company_Accreditation_ID { get; set; }

    public byte CommitteeType_ID { get; set; }

    /// <summary>
    /// تاريخ الفحص
    /// </summary>
    public DateOnly? Delegation_Date { get; set; }

    /// <summary>
    /// تاريخ الانتداب
    /// </summary>
    public DateOnly? Check_Date { get; set; }

    /// <summary>
    ///  بداية ساعة الفحص 
    /// </summary>
    public TimeOnly? StartTime { get; set; }

    /// <summary>
    /// انتهاء ساعة الفحص
    /// </summary>
    public TimeOnly? EndTime { get; set; }

    /// <summary>
    /// 0 if exporter doesn&apos;t accept else 1
    /// </summary>
    public bool IsApproved { get; set; }

    /// <summary>
    /// تم الانتهاء من الدفع
    /// </summary>
    public bool IsPaid { get; set; }

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

    public virtual CommitteeType CommitteeType { get; set; } = null!;

    public virtual ICollection<CompanyAccreditation_Payment> CompanyAccreditation_Payments { get; set; } = new List<CompanyAccreditation_Payment>();

    public virtual CompanyAccreditation Company_Accreditation { get; set; } = null!;
}
