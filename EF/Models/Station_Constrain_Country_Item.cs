using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// انواع الاشتراطات
/// </summary>
public partial class Station_Constrain_Country_Item
{
    public byte ID { get; set; }

    public byte Station_Type_ID { get; set; }

    /// <summary>
    /// الاسم بالعربية
    /// </summary>
    public string? Ar_Name { get; set; }

    /// <summary>
    /// الاسم بالانجليزية
    /// </summary>
    public string? En_Name { get; set; }

    /// <summary>
    /// مفعل
    /// </summary>
    public bool IsActive { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public virtual ICollection<Station_CheckList> Station_CheckLists { get; set; } = new List<Station_CheckList>();

    public virtual Station_Constrain_Type Station_Type { get; set; } = null!;
}
