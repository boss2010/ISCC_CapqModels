using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// نوع المرفق
/// </summary>
public partial class A_AttachmentTableType
{
    public short ID { get; set; }

    public string? Ar_Name { get; set; }

    public string? En_Name { get; set; }

    public bool? IsActive { get; set; }

    public int? opration_type_attachment { get; set; }

    public virtual ICollection<A_AttachmentDatum> A_AttachmentData { get; set; } = new List<A_AttachmentDatum>();

    public virtual ICollection<A_AttachmentData_Ex_CheckRequest> A_AttachmentData_Ex_CheckRequests { get; set; } = new List<A_AttachmentData_Ex_CheckRequest>();

    public virtual ICollection<A_AttachmentData_Station> A_AttachmentData_Stations { get; set; } = new List<A_AttachmentData_Station>();

    public virtual ICollection<Ex_CertificatesRequestsFile> Ex_CertificatesRequestsFiles { get; set; } = new List<Ex_CertificatesRequestsFile>();
}
