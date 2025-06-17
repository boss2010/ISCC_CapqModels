using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// المناطق
/// </summary>
public partial class Region
{
    public long ID { get; set; }

    /// <summary>
    /// الدولة
    /// </summary>
    public short? Country_ID { get; set; }

    public string? Name_En { get; set; }

    public string? Name_Ar { get; set; }

    public string? Descreption_En { get; set; }

    public string? Descreption_Ar { get; set; }

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

    public virtual Country? Country { get; set; }

    public virtual ICollection<Port_International> Port_Internationals { get; set; } = new List<Port_International>();
}
