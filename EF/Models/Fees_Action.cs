using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// تفاصيل الاجراءات
/// </summary>
public partial class Fees_Action
{
    public long ID { get; set; }

    public byte? FeesType_Id { get; set; }

    public byte? Feer_Type_Action_ID { get; set; }

    public long? Item_Shift_Treatment_ID { get; set; }

    public string? Name_En { get; set; }

    public string? Name_Ar { get; set; }

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

    public bool IsActive { get; set; }

    public bool IsMandatory { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public byte? Calculator_Type { get; set; }

    /// <summary>
    /// نوع الحساب من  system code رقم 33
    /// </summary>
    public int? Account_Type { get; set; }

    public virtual Fees_Type_Action? Feer_Type_Action { get; set; }

    public virtual FeesType? FeesType { get; set; }

    public virtual ICollection<Fees_Transactions_Detile> Fees_Transactions_Detiles { get; set; } = new List<Fees_Transactions_Detile>();
}
