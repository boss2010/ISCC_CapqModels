using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// المراكز
/// </summary>
public partial class Center
{
    public short ID { get; set; }

    /// <summary>
    /// المحافظة
    /// </summary>
    public short? Govern_ID { get; set; }

    /// <summary>
    /// الاسم بالعربية
    /// </summary>
    public string? Ar_Name { get; set; }

    /// <summary>
    /// الاسم بالانجليزية
    /// </summary>
    public string? En_Name { get; set; }

    /// <summary>
    /// مفعل
    /// </summary>
    public bool IsActive { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    /// <summary>
    /// المنافذ
    /// </summary>
    public long? Outlet_ID { get; set; }

    public virtual ICollection<Company_National> Company_Nationals { get; set; } = new List<Company_National>();

    public virtual ICollection<Ex_CheckRequest_Item> Ex_CheckRequest_Items { get; set; } = new List<Ex_CheckRequest_Item>();

    public virtual ICollection<Ex_CheckRequest_Place> Ex_CheckRequest_Places { get; set; } = new List<Ex_CheckRequest_Place>();

    public virtual ICollection<FarmsDatum> FarmsData { get; set; } = new List<FarmsDatum>();

    public virtual Governate? Govern { get; set; }

    public virtual Outlet? Outlet { get; set; }

    public virtual ICollection<Village> Villages { get; set; } = new List<Village>();
}
