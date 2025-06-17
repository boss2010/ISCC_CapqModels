using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Ex_CheckRequest_Organization_Distribution
{
    public long ID { get; set; }

    public long Ex_CheckRequest_ID { get; set; }

    /// <summary>
    /// رقم الجهة
    /// </summary>
    public long Organization_ID { get; set; }

    /// <summary>
    /// نوع الجهة
    /// </summary>
    public int Organization_Type_Id { get; set; }

    /// <summary>
    /// الكمية الصالحة للتصدير
    /// </summary>
    public double? Quantity_Ton { get; set; }

    public bool? IsActive { get; set; }

    public long? User_Creation_Id { get; set; }

    public DateTime? User_Creation_Date { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public long? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public long? User_Updation_Id { get; set; }

    public virtual Ex_CheckRequest Ex_CheckRequest { get; set; } = null!;
}
