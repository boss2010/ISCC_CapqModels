using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// المحافظة
/// </summary>
public partial class Governate
{
    public short ID { get; set; }

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

    public virtual ICollection<Center> Centers { get; set; } = new List<Center>();

    public virtual ICollection<Ex_CheckRequest_Item> Ex_CheckRequest_Items { get; set; } = new List<Ex_CheckRequest_Item>();

    public virtual ICollection<Ex_CheckRequest_Place> Ex_CheckRequest_Places { get; set; } = new List<Ex_CheckRequest_Place>();

    public virtual ICollection<FarmsDatum> FarmsData { get; set; } = new List<FarmsDatum>();

    public virtual ICollection<FreeZone> FreeZones { get; set; } = new List<FreeZone>();

    public virtual ICollection<PortNational> PortNationals { get; set; } = new List<PortNational>();
}
