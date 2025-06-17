using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// الاتحاد 
/// </summary>
public partial class Union
{
    public short ID { get; set; }

    /// <summary>
    /// الاسم بالعربية
    /// </summary>
    public string Ar_Name { get; set; } = null!;

    /// <summary>
    /// الاسم بالانجليزية
    /// </summary>
    public string En_Name { get; set; } = null!;

    public bool IsActive { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short? User_Creation_Id { get; set; }

    public DateTime? User_Creation_Date { get; set; }

    public virtual ICollection<Union_Country> Union_Countries { get; set; } = new List<Union_Country>();
}
