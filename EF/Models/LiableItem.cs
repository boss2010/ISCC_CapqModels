using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
///  
/// </summary>
public partial class LiableItem
{
    public int ID { get; set; }

    public string? Name_Ar { get; set; }

    public string? Name_En { get; set; }

    /// <summary>
    /// كائنات حية/غير حية
    /// </summary>
    public int IsAlive { get; set; }

    public short? LiableItems_Strain_Id { get; set; }

    public bool IsActive { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    /// <summary>
    /// هل له اذن استيراد -خاص بالوارد
    /// </summary>
    public bool? IsPermissionRequest { get; set; }

    public virtual A_SystemCode IsAliveNavigation { get; set; } = null!;

    public virtual ICollection<LiableItems_ShortName> LiableItems_ShortNames { get; set; } = new List<LiableItems_ShortName>();

    public virtual ItemCategories_Type? LiableItems_Strain { get; set; }
}
