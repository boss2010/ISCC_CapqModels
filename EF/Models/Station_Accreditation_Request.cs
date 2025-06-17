using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// اعتماد المحطة
/// </summary>
public partial class Station_Accreditation_Request
{
    public long ID { get; set; }

    /// <summary>
    /// المحطة
    /// </summary>
    public long Station_ID { get; set; }

    /// <summary>
    /// المحطة
    /// </summary>
    public long Station_Accreditation_Data_ID { get; set; }

    public byte Station_Accreditation_Request_Type_ID { get; set; }

    public string? Notes_Quarantine { get; set; }

    /// <summary>
    /// تاريخ البداية
    /// </summary>
    public DateOnly? StartDate { get; set; }

    /// <summary>
    /// تاريخ النهاية
    /// </summary>
    public DateOnly? EndDate { get; set; }

    public bool? IsActive { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    /// <summary>
    /// تم الانتهاء من الدفع
    /// </summary>
    public bool? IsPaid { get; set; }

    /// <summary>
    /// المبلغ
    /// </summary>
    public decimal? Amount_Total { get; set; }

    public byte? CommitteeType_ID { get; set; }

    /// <summary>
    /// موافقة ورفض الطلب
    /// 
    /// </summary>
    public bool? IsAccepted { get; set; }

    /// <summary>
    /// الموقف النهائي للطلب
    /// null لم يتم العمل على الطلب
    /// 0 يتم العمل على الطلب
    /// 1 تم الانتهاء من العمل على الطلب
    /// </summary>
    public bool? Is_Final_requst { get; set; }

    public virtual CommitteeType? CommitteeType { get; set; }

    public virtual Station Station { get; set; } = null!;

    public virtual ICollection<Station_Accreditation_Committee> Station_Accreditation_Committees { get; set; } = new List<Station_Accreditation_Committee>();

    public virtual Station_Accreditation_Datum Station_Accreditation_Data { get; set; } = null!;

    public virtual ICollection<Station_Accreditation_Request_Fee> Station_Accreditation_Request_Fees { get; set; } = new List<Station_Accreditation_Request_Fee>();

    public virtual Station_Accreditation_Request_Type Station_Accreditation_Request_Type { get; set; } = null!;
}
