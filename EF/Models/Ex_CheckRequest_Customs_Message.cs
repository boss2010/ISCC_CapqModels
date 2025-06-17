using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Ex_CheckRequest_Customs_Message
{
    public int ID { get; set; }

    public long? Ex_CheckRequest_ID { get; set; }

    public long? Ex_CertificatesRequests_ID { get; set; }

    public string? Customs_Certificate_Number { get; set; }

    public DateOnly? Certification_Date { get; set; }

    public DateOnly? Shipment_Date { get; set; }

    public DateOnly? Arrival_Date { get; set; }

    public string? Certificate_Number_Each_Product { get; set; }

    public string? Manifest_Number { get; set; }

    public long? Shipping_Agency_ID { get; set; }

    public byte? Im_OperationType { get; set; }

    public short? User_Creation_Id { get; set; }

    public DateTime? User_Creation_Date { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public bool? IsActive { get; set; }

    public virtual Ex_CertificatesRequest? Ex_CertificatesRequests { get; set; }

    public virtual Ex_CheckRequest? Ex_CheckRequest { get; set; }

    public virtual ShippingAgency? Shipping_Agency { get; set; }
}
