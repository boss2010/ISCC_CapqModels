using System;
using System.Collections.Generic;

namespace EF.Imports.Entities;

/// <summary>
/// الدول
/// </summary>
public partial class Country
{
    public short ID { get; set; }

    /// <summary>
    /// الاسم بالعربية
    /// </summary>
    public string? Ar_Name { get; set; }

    /// <summary>
    /// الاسم بالانجليزية
    /// </summary>
    public string? En_Name { get; set; }

    public bool Is_IPPC { get; set; }

    public bool IsActive { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short? User_Creation_Id { get; set; }

    public DateTime? User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public byte? Continents_ID { get; set; }

    public byte? Regional_Area_ID { get; set; }

    public virtual ICollection<Im_CheckRequest_Data> Im_CheckRequest_Data { get; set; } = new List<Im_CheckRequest_Data>();
}
