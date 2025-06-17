using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// المجموعة الزراعيه
/// </summary>
public partial class Group
{
    public int ID { get; set; }

    /// <summary>
    /// الاسم بالعربية
    /// </summary>
    public string? Name_Ar { get; set; }

    /// <summary>
    /// الاسم بالانجليزية
    /// </summary>
    public string? Name_En { get; set; }

    public int? SecClass_ID { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short? User_Creation_Id { get; set; }

    public DateTime? User_Creation_Date { get; set; }

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();

    public virtual SecondaryClassification? SecClass { get; set; }
}
