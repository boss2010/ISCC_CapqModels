using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Ex_CertificatesRequestsPaymentsDetaile
{
    public long ID { get; set; }

    public long? Ex_CertificatesRequestsPaymentsID { get; set; }

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
    /// from systemcode table 30
    /// نوع عملية الدفع فيزا - كاش
    /// </summary>
    public int? Payment_Type_ID { get; set; }

    public long? User_Creation_Id { get; set; }

    public DateTime? User_Creation_Date { get; set; }

    public virtual Ex_CertificatesRequestsPayment? Ex_CertificatesRequestsPayments { get; set; }
}
