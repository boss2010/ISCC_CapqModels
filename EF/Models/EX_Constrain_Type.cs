using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
///  الاشتراطات
/// </summary>
public partial class EX_Constrain_Type
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
    /// مفعل
    /// </summary>
    public bool IsActive { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public virtual ICollection<EX_Constrain_Country_Item> EX_Constrain_Country_Items { get; set; } = new List<EX_Constrain_Country_Item>();
}
