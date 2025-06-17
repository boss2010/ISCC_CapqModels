using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class PublicOrganization_Type
{
    public int ID { get; set; }

    /// <summary>
    /// الاسم بالانجليزية
    /// </summary>
    public string? Name_En { get; set; }

    /// <summary>
    /// الاسم بالعربية
    /// </summary>
    public string? Name_Ar { get; set; }

    /// <summary>
    /// معفى من طلب إذن استيراد
    /// </summary>
    public bool IsExempt { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public virtual ICollection<Public_Organization> Public_Organizations { get; set; } = new List<Public_Organization>();
}
