using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// المخازن
/// </summary>
public partial class Im_Store
{
    public int ID { get; set; }

    public string? Name_Ar { get; set; }

    public string? Name_En { get; set; }

    /// <summary>
    /// مخزن/ساحة
    /// </summary>
    public string? Place { get; set; }

    public string? StoreOwner { get; set; }

    public decimal? Phone { get; set; }

    public decimal? Fax { get; set; }

    public string? Email { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }
}
