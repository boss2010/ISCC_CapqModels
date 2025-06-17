using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// القرية
/// </summary>
public partial class Village
{
    public short ID { get; set; }

    /// <summary>
    /// المركز
    /// </summary>
    public short? Center_ID { get; set; }

    /// <summary>
    /// الاسم بالعربية
    /// </summary>
    public string? Ar_Name { get; set; }

    /// <summary>
    /// الاسم بالانجليزية
    /// </summary>
    public string? En_Name { get; set; }

    public bool IsActive { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public virtual Center? Center { get; set; }

    public virtual ICollection<Company_National> Company_Nationals { get; set; } = new List<Company_National>();

    public virtual ICollection<Ex_CheckRequest_Item> Ex_CheckRequest_Items { get; set; } = new List<Ex_CheckRequest_Item>();

    public virtual ICollection<FarmsDatum> FarmsData { get; set; } = new List<FarmsDatum>();
}
