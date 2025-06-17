using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// الشروط الاضافية الجديدة
/// </summary>
public partial class Ex_CertificateAddtionUser
{
    public long ID { get; set; }

    public long? PlantCertificatesRequestsID { get; set; }

    public string? Certificate_AddtionText { get; set; }

    /// <summary>
    /// اضافه العميل 1 او رد الحجر 0
    /// 
    /// </summary>
    public bool? IS_Client_OR_Agree { get; set; }

    public virtual Ex_CertificatesRequest? PlantCertificatesRequests { get; set; }
}
