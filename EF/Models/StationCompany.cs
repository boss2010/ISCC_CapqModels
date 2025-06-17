using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// شركات المحطة
/// </summary>
public partial class StationCompany
{
    public long ID { get; set; }

    public long? Company_ID { get; set; }

    public long? Station_Accreditation_ID { get; set; }

    public DateOnly? Start_Date { get; set; }

    public DateOnly? End_Date { get; set; }

    public bool? IsActive { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    /// <summary>
    /// 2 تحت الدراسة
    /// 1 مقبول
    /// 0 مرفوض
    /// 3 ايقاف
    /// 
    /// 
    /// 
    /// </summary>
    public byte? Status { get; set; }

    /// <summary>
    /// from systemcode table 3
    /// </summary>
    public int? Company_Type_Id { get; set; }

    public virtual Station_Accreditation? Station_Accreditation { get; set; }
}
