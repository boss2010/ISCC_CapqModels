using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Ex_CheckRequest_Organization_Distribution_Master
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

    public long Item_ID { get; set; }

    public long Item_ShortName_ID { get; set; }

    /// <summary>
    /// الكمية الصالحة للتصدير
    /// </summary>
    public double Totall_Quantity_Ton_Ex_CheckRequest { get; set; }

    public bool IsActive { get; set; }

    public long User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public long? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public long? User_Updation_Id { get; set; }

    public double? Registered_Quantity_Ton_Ex_CheckRequest { get; set; }

    public virtual ICollection<Ex_CheckRequest_Organization_Distribution_Detial> Ex_CheckRequest_Organization_Distribution_Detials { get; set; } = new List<Ex_CheckRequest_Organization_Distribution_Detial>();

    public virtual Item_ShortName Item_ShortName { get; set; } = null!;
}
