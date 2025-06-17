using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// المستوي
/// </summary>
public partial class Level
{
    public int ID { get; set; }

    public string? Name_Ar { get; set; }

    /// <summary>
    /// الاسم بالانجليزية
    /// </summary>
    public string? Name_En { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short? User_Creation_Id { get; set; }

    public DateTime? User_Creation_Date { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<PhylumSubphylum> PhylumSubphylums { get; set; } = new List<PhylumSubphylum>();
}
