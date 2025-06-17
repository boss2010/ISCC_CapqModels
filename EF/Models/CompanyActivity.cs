using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// نشاط الشركة
/// </summary>
public partial class CompanyActivity
{
    public long ID { get; set; }

    /// <summary>
    /// الشركة
    /// </summary>
    public long? Company_ID { get; set; }

    /// <summary>
    /// نوع النشاط
    /// </summary>
    public byte? CompActivityType_ID { get; set; }

    /// <summary>
    /// نوع نشاط الرئيسى from systemcode 17
    /// </summary>
    public int MainActivityType { get; set; }

    public string? Enrollment_Name { get; set; }

    public decimal? Enrollment_Number { get; set; }

    /// <summary>
    /// تاريخ بداية
    /// </summary>
    public DateOnly? Enrollment_Start { get; set; }

    /// <summary>
    /// تاريخ نهاية
    /// </summary>
    public DateOnly? Enrollment_End { get; set; }

    public bool IsActive { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public byte? Enrollment_type_ID { get; set; }

    public virtual CompanyActivityType? CompActivityType { get; set; }

    public virtual Company_National? Company { get; set; }

    public virtual Enrollment_type? Enrollment_type { get; set; }

    public virtual A_SystemCode MainActivityTypeNavigation { get; set; } = null!;
}
