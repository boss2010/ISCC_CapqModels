using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// المعالجة الرئيسية
/// </summary>
public partial class TreatmentMainType
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

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public virtual ICollection<TreatmentType> TreatmentTypes { get; set; } = new List<TreatmentType>();
}
