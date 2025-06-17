using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// نوع العبوة
/// </summary>
public partial class Package_Type
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

    public bool IsActive { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short? User_Creation_Id { get; set; }

    public DateTime? User_Creation_Date { get; set; }

    public virtual ICollection<Im_ScientificResearch_ItemPlant_Inseket_Lieble> Im_ScientificResearch_ItemPlant_Inseket_Liebles { get; set; } = new List<Im_ScientificResearch_ItemPlant_Inseket_Lieble>();
}
