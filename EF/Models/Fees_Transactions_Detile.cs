using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Fees_Transactions_Detile
{
    public long ID { get; set; }

    public long? Fees_Action_ID { get; set; }

    public long? Fees_Transactions_ID { get; set; }

    public long? Items_ID { get; set; }

    public long? Shift_ID { get; set; }

    public long? SampleData_ID { get; set; }

    /// <summary>
    /// جاي من جدول Im_Request_TreatmentData عشان لو اكثر من لوط هيبقى كل لوط له id مختلف
    /// </summary>
    public long? TreatmentData_ID { get; set; }

    /// <summary>
    /// المبلغ
    /// </summary>
    public decimal? Amount { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public virtual Fees_Action? Fees_Action { get; set; }

    public virtual Fees_Transaction? Fees_Transactions { get; set; }
}
