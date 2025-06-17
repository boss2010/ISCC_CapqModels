using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// تحصيل الرسوم الخاصه بالشهاده
/// </summary>
public partial class Ex_CertificatesRequestsPayment
{
    public long ID { get; set; }

    public long PlantCertificatesRequestsID { get; set; }

    public long? User_Creation_Id { get; set; }

    public DateTime? User_Creation_Date { get; set; }

    public double? Value { get; set; }

    public byte? Ex_CertificatesRequestsPaymentsType { get; set; }

    public bool? IsPayment { get; set; }

    public virtual ICollection<Ex_CertificatesRequestsPaymentsDetaile> Ex_CertificatesRequestsPaymentsDetailes { get; set; } = new List<Ex_CertificatesRequestsPaymentsDetaile>();

    public virtual Ex_CertificatesRequestsPaymentsType? Ex_CertificatesRequestsPaymentsTypeNavigation { get; set; }

    public virtual Ex_CertificatesRequest PlantCertificatesRequests { get; set; } = null!;
}
