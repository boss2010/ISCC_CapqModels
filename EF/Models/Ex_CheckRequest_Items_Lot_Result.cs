using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Ex_CheckRequest_Items_Lot_Result
{
    public long ID { get; set; }

    public long Ex_CheckRequest_Items_Lot_Category_ID { get; set; }

    /// <summary>
    /// الموقف مقبول او مرفوض
    /// </summary>
    public bool? IS_Status_Committee { get; set; }

    public int? IS_Status { get; set; }

    public string? Nots { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short? User_Creation_Id { get; set; }

    public DateTime? User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public virtual Ex_CheckRequest_Items_Lot_Category Ex_CheckRequest_Items_Lot_Category { get; set; } = null!;

    public virtual Ex_CheckRequest_Lot_Result_Status? IS_StatusNavigation { get; set; }
}
