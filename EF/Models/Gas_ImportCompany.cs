using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// شركات استيراد الغاز
/// </summary>
public partial class Gas_ImportCompany
{
    public long ID { get; set; }

    public long? Company_ID { get; set; }

    /// <summary>
    /// كمية الغاز المستوردة
    /// </summary>
    public decimal? GasAmount { get; set; }

    /// <summary>
    /// تاريخ الموافقة
    /// </summary>
    public DateOnly? AcceptanceDate { get; set; }

    public bool IsActive { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public virtual Company_National? Company { get; set; }

    public virtual ICollection<SteamingCompany> SteamingCompanies { get; set; } = new List<SteamingCompany>();
}
