using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// شركات التبخير
/// </summary>
public partial class SteamingCompany
{
    public long ID { get; set; }

    /// <summary>
    /// شركة المكافحة
    /// </summary>
    public long? Company_ID { get; set; }

    /// <summary>
    /// كمية الغاز المنصرف للمعالجة
    /// </summary>
    public decimal? OutGas_Amount { get; set; }

    /// <summary>
    /// شركة استيراد الغاز
    /// </summary>
    public long? GasCompany_ID { get; set; }

    /// <summary>
    /// تاريخ الصرف
    /// </summary>
    public DateOnly? OutGas_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public virtual Company_National? Company { get; set; }

    public virtual Gas_ImportCompany? GasCompany { get; set; }
}
