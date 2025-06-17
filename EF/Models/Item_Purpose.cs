using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// الغرض
/// </summary>
public partial class Item_Purpose
{
    public int ID { get; set; }

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

    public bool IsActive { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public byte? Item_Type_ID { get; set; }

    public virtual ICollection<Item_ShortName> Item_ShortNames { get; set; } = new List<Item_ShortName>();

    public virtual ICollection<LiableItems_ShortName> LiableItems_ShortNames { get; set; } = new List<LiableItems_ShortName>();
}
