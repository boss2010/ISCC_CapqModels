using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Pallet_Data_Organization__Distribution
{
    public long ID { get; set; }

    /// <summary>
    /// رقم الجهة المشتري
    /// </summary>
    public long Organization_ID { get; set; }

    /// <summary>
    /// نوع الجهة المشتري
    /// </summary>
    public int Organization_Type_Id { get; set; }

    /// <summary>
    /// كمية البالتات
    /// </summary>
    public int? Quantity { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public DateOnly Date { get; set; }

    public bool IsActive { get; set; }

    public bool? IsAcceppted { get; set; }

    public long User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public long? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public long? User_Updation_Id { get; set; }

    public long Ex_CheckRequest_ID { get; set; }

    public virtual Ex_CheckRequest Ex_CheckRequest { get; set; } = null!;

    public virtual ICollection<Pallet_Data_Ex_CheckRequest_Distribution> Pallet_Data_Ex_CheckRequest_Distributions { get; set; } = new List<Pallet_Data_Ex_CheckRequest_Distribution>();
}
