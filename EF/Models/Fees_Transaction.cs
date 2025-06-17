using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Fees_Transaction
{
    public long ID { get; set; }

    /// <summary>
    /// id table (fees_tablename)
    /// </summary>
    public short? TableName_ID { get; set; }

    /// <summary>
    /// id الجدول الرئيسي اللي متحدد في (fess_tablename)
    /// </summary>
    public long? Table_ID { get; set; }

    /// <summary>
    /// المبلغ
    /// </summary>
    public decimal? Amount_Total { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    /// <summary>
    /// from systemcode table 30
    /// نوع عملية الدفع فيزا - كاش
    /// </summary>
    public int? Payment_Type_ID { get; set; }

    public string? OrderNumber { get; set; }

    /// <summary>
    /// 0 تم رفض عملية البنك
    /// 1 تم قبول العملية 
    /// null تم الارسال ولم الرد من البنك
    /// </summary>
    public bool? IsSuccess_Bank { get; set; }

    /// <summary>
    /// كود العملية من البنك
    /// </summary>
    public string? Code_Bank { get; set; }

    /// <summary>
    /// from systemcode table 3
    /// نوع الموظف حجر ولا شركة ولا فرد ولاهيئه
    /// </summary>
    public int? User_Type_ID { get; set; }

    public DateOnly? date { get; set; }

    /// <summary>
    /// نوع الحساب من system code رقم 33
    /// </summary>
    public int? Account_Type { get; set; }

    public virtual ICollection<Fees_Transactions_Detile> Fees_Transactions_Detiles { get; set; } = new List<Fees_Transactions_Detile>();

    public virtual Fees_TableName? TableName { get; set; }
}
