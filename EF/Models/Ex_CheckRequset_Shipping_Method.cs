using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Ex_CheckRequset_Shipping_Method
{
    public long ID { get; set; }

    public long? Ex_CheckRequest_ID { get; set; }

    public int? containers_ID { get; set; }

    public int? containers_type_ID { get; set; }

    public string? ShipholdNumber { get; set; }

    public string? ContainerNumber { get; set; }

    public string? NavigationalNumber { get; set; }

    public decimal? Total_Weight { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public virtual ICollection<Ex_CertificatesRequestsLotDatum> Ex_CertificatesRequestsLotData { get; set; } = new List<Ex_CertificatesRequestsLotDatum>();

    public virtual ICollection<Ex_CheckRequest_Items_Lot_Category> Ex_CheckRequest_Items_Lot_Categories { get; set; } = new List<Ex_CheckRequest_Items_Lot_Category>();
}
