using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// مجموعة النوعية
/// </summary>
public partial class QualitativeGroup
{
    public short Id { get; set; }

    /// <summary>
    /// وصف أو تنويه
    /// </summary>
    public string? Descreption_En { get; set; }

    /// <summary>
    /// الاسم بالعربية
    /// </summary>
    public string? Name_Ar { get; set; }

    /// <summary>
    /// الاسم بالانجليزية
    /// </summary>
    public string? Name_En { get; set; }

    /// <summary>
    /// وصف أو تنويه
    /// </summary>
    public string? Descreption_Ar { get; set; }

    public bool IsActive { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public bool? IsPallet { get; set; }

    public virtual ICollection<Im_CountryConstrain_ArrivalPort> Im_CountryConstrain_ArrivalPorts { get; set; } = new List<Im_CountryConstrain_ArrivalPort>();

    public virtual ICollection<Item_ShortName> Item_ShortNames { get; set; } = new List<Item_ShortName>();
}
