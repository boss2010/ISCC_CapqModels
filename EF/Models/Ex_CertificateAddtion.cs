using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// اضافات  الاشتراطات للشهادة
/// </summary>
public partial class Ex_CertificateAddtion
{
    public long ID { get; set; }

    public long? PlantCertificatesRequestsID { get; set; }

    /// <summary>
    /// نص الاشتراط الاصلى
    /// </summary>
    public string? Certificate_AddtionOriginal { get; set; }

    /// <summary>
    /// نص الاشتراطات بعد التعديل
    /// </summary>
    public string? Certificate_AddtionOriginalUpdate { get; set; }

    /// <summary>
    /// رقم الاشتراط الاصلى
    /// </summary>
    public long? ConstrainID { get; set; }

    /// <summary>
    /// رقم موظف الحجر
    /// </summary>
    public long? AdminID { get; set; }

    /// <summary>
    /// الاشتراط للحجر بعد التعديل
    /// </summary>
    public string? Certificate_AddtionUpdateAdmin { get; set; }

    public bool? ISAccepted { get; set; }

    public DateTime? Date_Accepted { get; set; }

    public virtual Ex_CertificatesRequest? PlantCertificatesRequests { get; set; }
}
