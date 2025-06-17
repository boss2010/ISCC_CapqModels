using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// البيانات الجمركيه
/// </summary>
public partial class Im_CheckRequest_Customs_Message
{
    public int ID { get; set; }

    /// <summary>
    /// كود الطلب
    /// </summary>
    public long? Im_CheckRequest_ID { get; set; }

    /// <summary>
    /// رقم الشهاده الجمركيه
    /// </summary>
    public string? Customs_Certificate_Number { get; set; }

    /// <summary>
    /// تاريخ الشهاده الجمركيه
    /// </summary>
    public DateOnly? Certification_Date { get; set; }

    /// <summary>
    /// تاريخ الشحن
    /// </summary>
    public DateOnly? Shipment_Date { get; set; }

    /// <summary>
    /// تاريخ الوصول 
    /// </summary>
    public DateOnly? Arrival_Date { get; set; }

    /// <summary>
    /// رقم الشهادة لكل منتج
    /// </summary>
    public string? Certificate_Number_Each_Product { get; set; }

    public string? Manifest_Number { get; set; }

    public long? Shipping_Agency_ID { get; set; }

    /// <summary>
    /// نوع الطلب
    /// </summary>
    public byte? Im_OperationType { get; set; }

    public short? User_Creation_Id { get; set; }

    public DateTime? User_Creation_Date { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public bool? IsActive { get; set; }

    public virtual Im_CheckRequest? Im_CheckRequest { get; set; }

    public virtual ShippingAgency? Shipping_Agency { get; set; }
}
