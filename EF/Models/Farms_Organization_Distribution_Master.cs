using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Farms_Organization_Distribution_Master
{
    public long ID { get; set; }

    /// <summary>
    /// رقم الجهة
    /// </summary>
    public long Organization_ID { get; set; }

    /// <summary>
    /// نوع الجهة
    /// </summary>
    public int Organization_Type_Id { get; set; }

    public long FarmsData_ID { get; set; }

    public long Farm_ItemCategories_ID { get; set; }

    public long Item_ID { get; set; }

    public long ItemCategories_ID { get; set; }

    /// <summary>
    /// الكمية الصالحة للتصدير
    /// </summary>
    public double Quantity_Ton_Farm { get; set; }

    public bool IsActive { get; set; }

    public long User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public long? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public long? User_Updation_Id { get; set; }

    public double? Quantity_Ton_Ex_CheckRequest { get; set; }

    public virtual Farm_ItemCategory Farm_ItemCategories { get; set; } = null!;

    public virtual ICollection<Farms_Organization_Distribution_Detial> Farms_Organization_Distribution_Detials { get; set; } = new List<Farms_Organization_Distribution_Detial>();
}
