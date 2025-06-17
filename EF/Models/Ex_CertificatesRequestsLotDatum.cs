using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// بيانات اللوطات
/// </summary>
public partial class Ex_CertificatesRequestsLotDatum
{
    public long ID { get; set; }

    public long? PlantCertificatesRequestsID { get; set; }

    public long? LotID { get; set; }

    public long? Item_ShortName_ID { get; set; }

    public bool? ISAccepted { get; set; }

    public long? Ex_CheckRequset_Shipping_MethodID { get; set; }

    public virtual Ex_CheckRequset_Shipping_Method? Ex_CheckRequset_Shipping_Method { get; set; }

    public virtual Ex_CertificatesRequest? PlantCertificatesRequests { get; set; }
}
