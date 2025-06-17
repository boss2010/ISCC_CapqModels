using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// التاشيره
/// </summary>
public partial class Im_Visa
{
    public long ID { get; set; }

    /// <summary>
    /// الاسم بالعربية
    /// </summary>
    public string? Ar_Name { get; set; }

    /// <summary>
    /// الاسم بالانجليزية
    /// </summary>
    public string? En_Name { get; set; }

    public short? User_Creation_Id { get; set; }

    public DateTime? User_Creation_Date { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public bool? IsActive { get; set; }

    public string? Description_Ar { get; set; }

    public string? Description_En { get; set; }

    public virtual ICollection<Ex_CheckRequest_Visa> Ex_CheckRequest_Visas { get; set; } = new List<Ex_CheckRequest_Visa>();

    public virtual ICollection<Im_CheckRequest_Visa> Im_CheckRequest_Visas { get; set; } = new List<Im_CheckRequest_Visa>();
}
