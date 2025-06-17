using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class FeesAmount_Fixed
{
    public int ID { get; set; }

    public string? Name_En { get; set; }

    public string? Name_Ar { get; set; }

    public byte FeesType_Id { get; set; }

    /// <summary>
    /// المبلغ
    /// </summary>
    public decimal? Amount { get; set; }

    /// <summary>
    /// الحد الادنى
    /// </summary>
    public decimal? MinAmount { get; set; }

    public double? WeightFrom { get; set; }

    /// <summary>
    /// الوزن
    /// </summary>
    public double? WeightTo { get; set; }

    /// <summary>
    /// هل تدفع عند الطلب ام بعده
    /// </summary>
    public bool? IsPaidBefore { get; set; }

    public bool IsExport { get; set; }

    public bool IsActive { get; set; }

    public bool IsMandatory { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public virtual FeesType FeesType { get; set; } = null!;
}
