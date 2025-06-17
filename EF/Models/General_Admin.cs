using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// الإدارة العامة
/// </summary>
public partial class General_Admin
{
    public byte ID { get; set; }

    /// <summary>
    /// الاسم بالعربية
    /// </summary>
    public string? Ar_Name { get; set; }

    /// <summary>
    /// الاسم بالانجليزية
    /// </summary>
    public string? En_Name { get; set; }

    /// <summary>
    /// العنوان بالعربية
    /// </summary>
    public string? Address_Ar { get; set; }

    /// <summary>
    /// العنوان بالانجليزية
    /// </summary>
    public string? Address_En { get; set; }

    /// <summary>
    /// رئيس/مدير الإدارة
    /// from HR employee table
    /// </summary>
    public int? Admin_ID { get; set; }

    public bool IsActive { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public long? ID_Orcael { get; set; }

    public long? HR_SECTOR_NO { get; set; }

    public virtual ICollection<Outlet> Outlets { get; set; } = new List<Outlet>();
}
