using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// نوع بيان الاتصال
/// </summary>
public partial class ContactType
{
    public byte ID { get; set; }

    /// <summary>
    /// الاسم بالعربية
    /// </summary>
    public string? Name_Ar { get; set; }

    /// <summary>
    /// الاسم بالانجليزية
    /// </summary>
    public string? Name_En { get; set; }

    public bool IsActive { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public virtual ICollection<Ex_ContactDatum> Ex_ContactData { get; set; } = new List<Ex_ContactDatum>();

    public virtual ICollection<HagrContact> HagrContacts { get; set; } = new List<HagrContact>();

    public virtual ICollection<StationContact> StationContacts { get; set; } = new List<StationContact>();
}
