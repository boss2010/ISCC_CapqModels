using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Regional_Area
{
    /// <summary>
    /// المناطق الاقليمية
    /// </summary>
    public byte ID { get; set; }

    public string? Name_En { get; set; }

    public string? Name_Ar { get; set; }

    /// <summary>
    /// قارة
    /// </summary>
    public bool IsActive { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public virtual ICollection<Country> Countries { get; set; } = new List<Country>();
}
