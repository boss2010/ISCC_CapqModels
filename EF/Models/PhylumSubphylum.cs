using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// الشعبه
/// </summary>
public partial class PhylumSubphylum
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

    public int? Kingdom_ID { get; set; }

    public int? Level_ID { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public virtual Kingdom? Kingdom { get; set; }

    public virtual Level? Level { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
