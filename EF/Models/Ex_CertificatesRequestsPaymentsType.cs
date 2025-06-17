using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Ex_CertificatesRequestsPaymentsType
{
    public byte ID { get; set; }

    public string? Name { get; set; }

    public double? Value { get; set; }

    public virtual ICollection<Ex_CertificatesRequestsPayment> Ex_CertificatesRequestsPayments { get; set; } = new List<Ex_CertificatesRequestsPayment>();
}
