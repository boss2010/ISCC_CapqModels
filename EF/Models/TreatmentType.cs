using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// أنواع معالجات
/// </summary>
public partial class TreatmentType
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

    public byte MainType_ID { get; set; }

    public bool IsActive { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public virtual TreatmentMainType MainType { get; set; } = null!;

    public virtual ICollection<TreatmentMethod> TreatmentMethods { get; set; } = new List<TreatmentMethod>();
}
