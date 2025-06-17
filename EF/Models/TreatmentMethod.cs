using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// طرق المعالجة
/// </summary>
public partial class TreatmentMethod
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

    public byte? TreatmentType_ID { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public string? Desc_Ar { get; set; }

    public string? Desc_En { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<EX_Choose_Treatment> EX_Choose_Treatments { get; set; } = new List<EX_Choose_Treatment>();

    public virtual ICollection<Ex_CountryConstrain_Treatment> Ex_CountryConstrain_Treatments { get; set; } = new List<Ex_CountryConstrain_Treatment>();

    public virtual ICollection<StationActivityType> StationActivityTypes { get; set; } = new List<StationActivityType>();

    public virtual ICollection<TreatmentMaterial> TreatmentMaterials { get; set; } = new List<TreatmentMaterial>();

    public virtual TreatmentType? TreatmentType { get; set; }
}
