using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// الصنف الاساسى
/// </summary>
public partial class MainCalssification
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

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public byte? Item_Type_ID { get; set; }

    public virtual Item_Type? Item_Type { get; set; }

    public virtual ICollection<SecondaryClassification> SecondaryClassifications { get; set; } = new List<SecondaryClassification>();
}
