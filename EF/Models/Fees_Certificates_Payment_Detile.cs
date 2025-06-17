using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// تفاصيل الدفع للشهادات
/// </summary>
public partial class Fees_Certificates_Payment_Detile
{
    public long ID { get; set; }

    public string? AttachmentPath { get; set; }

    /// <summary>
    /// رقم البطاقه(pos) او المجموعه(كاش) علي حسب طريقه الدفع
    /// </summary>
    public string? CardOrGroupNumber { get; set; }

    /// <summary>
    /// رقم مرجعي او القسيمه
    /// </summary>
    public string? ReferenceOrCouponNumber { get; set; }

    /// <summary>
    /// تاريخ الدفع
    /// </summary>
    public DateOnly? PaymentDate { get; set; }

    public long? pos_information_id { get; set; }

    public long? Ex_CertificatesRequests_ID { get; set; }

    public virtual Ex_CertificatesRequest? Ex_CertificatesRequests { get; set; }
}
