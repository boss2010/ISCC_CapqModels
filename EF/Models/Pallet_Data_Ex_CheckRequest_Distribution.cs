using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Pallet_Data_Ex_CheckRequest_Distribution
{
    public long ID { get; set; }

    public long Ex_CheckRequest_ID { get; set; }

    public long? Pallet_Data_Organization__Distribution_ID { get; set; }

    /// <summary>
    /// الكمية الصالحة للتصدير
    /// </summary>
    public int Quantity { get; set; }

    public DateOnly? date { get; set; }

    public bool? IsActive { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public virtual Ex_CheckRequest Ex_CheckRequest { get; set; } = null!;

    public virtual Pallet_Data_Organization__Distribution? Pallet_Data_Organization__Distribution { get; set; }
}
