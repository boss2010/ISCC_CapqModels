using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// شركات الشحن الدولية
/// </summary>
public partial class ShippingCompany
{
    public long ID { get; set; }

    /// <summary>
    /// الاسم بالعربية
    /// </summary>
    public string? Name_Ar { get; set; }

    /// <summary>
    /// الاسم بالانجليزية
    /// </summary>
    public string? Name_En { get; set; }

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public bool IsActive { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public virtual ICollection<Ex_CheckRequest_Datum> Ex_CheckRequest_Data { get; set; } = new List<Ex_CheckRequest_Datum>();

    public virtual ICollection<Im_CheckRequest_Datum> Im_CheckRequest_Data { get; set; } = new List<Im_CheckRequest_Datum>();
}
