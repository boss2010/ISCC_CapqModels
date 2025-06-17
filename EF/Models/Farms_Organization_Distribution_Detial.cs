using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Farms_Organization_Distribution_Detial
{
    public long ID { get; set; }

    public long Farms_Organization_Distribution_Master_ID { get; set; }

    public DateOnly? date { get; set; }

    public bool? IsActive { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    /// <summary>
    /// الكمية الصالحة للتصدير
    /// </summary>
    public double Quantity_Ton { get; set; }

    public virtual Farms_Organization_Distribution_Master Farms_Organization_Distribution_Master { get; set; } = null!;
}
