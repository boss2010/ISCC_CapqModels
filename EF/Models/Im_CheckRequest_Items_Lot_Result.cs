using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// نتيجه اللوط
/// </summary>
public partial class Im_CheckRequest_Items_Lot_Result
{
    public long ID { get; set; }

    public long Im_CheckRequest_Items_Lot_Category_ID { get; set; }

    public string? Nots { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short? User_Creation_Id { get; set; }

    public DateTime? User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    /// <summary>
    /// الموقف مقبول او مرفوض
    /// </summary>
    public int? IS_Status { get; set; }

    /// <summary>
    /// الموقف مقبول او مرفوض
    /// </summary>
    public bool? IS_Status_Committee { get; set; }

    public virtual Im_CheckRequest_Lot_Result_Status? IS_StatusNavigation { get; set; }
}
