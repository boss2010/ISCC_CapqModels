using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// شروط الاندريد
/// </summary>
public partial class Station_CheckList
{
    public long ID { get; set; }

    public string? ConstrainText_Ar { get; set; }

    public string? ConstrainText_En { get; set; }

    public string? Description_Ar { get; set; }

    public string? Description_En { get; set; }

    public bool IsActive { get; set; }

    public long User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public long? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public long? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public string? Number_Check { get; set; }

    /// <summary>
    /// شهادة الصحة النباتية
    /// </summary>
    public bool Is_Androud { get; set; }

    public byte? Station_Constrain_Country_Item_ID { get; set; }

    public virtual ICollection<Station_Accreditation_CheckList> Station_Accreditation_CheckLists { get; set; } = new List<Station_Accreditation_CheckList>();

    public virtual Station_Constrain_Country_Item? Station_Constrain_Country_Item { get; set; }
}
