using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// هيئة الموانئ
/// </summary>
public partial class PortOrganization
{
    public int ID { get; set; }

    /// <summary>
    /// الاسم بالعربية
    /// </summary>
    public string? Ar_Name { get; set; }

    /// <summary>
    /// الاسم بالانجليزية
    /// </summary>
    public string? En_Name { get; set; }

    /// <summary>
    /// التليفون
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// الفاكس
    /// </summary>
    public string? Fax { get; set; }

    /// <summary>
    /// البريد الاليكتروني
    /// </summary>
    public string? Email { get; set; }

    public bool IsActive { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public virtual ICollection<PortNational> PortNationals { get; set; } = new List<PortNational>();
}
