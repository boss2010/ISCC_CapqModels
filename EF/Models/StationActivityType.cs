using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// أنواع أنشطة المحطة
/// </summary>
public partial class StationActivityType
{
    public byte ID { get; set; }

    /// <summary>
    /// الاسم بالعربية
    /// </summary>
    public string? Ar_Name { get; set; }

    /// <summary>
    /// الاسم بالانجليزية
    /// </summary>
    public string? En_Name { get; set; }

    /// <summary>
    /// وصف أو تنويه
    /// </summary>
    public string? Descreption_Ar { get; set; }

    /// <summary>
    /// وصف أو تنويه
    /// </summary>
    public string? Descreption_En { get; set; }

    public bool IsTreatment { get; set; }

    public bool IsActive { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public byte? TreatmentMethods_ID { get; set; }

    public virtual ICollection<Station_Accreditation_Datum> Station_Accreditation_Data { get; set; } = new List<Station_Accreditation_Datum>();

    public virtual TreatmentMethod? TreatmentMethods { get; set; }
}
