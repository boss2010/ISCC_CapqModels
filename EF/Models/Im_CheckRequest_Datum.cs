using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Im_CheckRequest_Datum
{
    public long ID { get; set; }

    public long Importer_ID { get; set; }

    public int ImporterType_Id { get; set; }

    public short ExportCountry_Id { get; set; }

    public byte? Shipment_Mean_Id { get; set; }

    public byte? Transport_Mean_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short? User_Creation_Id { get; set; }

    public DateTime? User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public long? Im_CheckRequest_ID { get; set; }

    public string? Ship_Name { get; set; }

    public string? DelegateName { get; set; }

    public string? DelegateAddress { get; set; }

    /// <summary>
    /// دولة العبور
    /// </summary>
    public short? TransitCountry_Id { get; set; }

    public long? InternationalTransportation_ID { get; set; }

    public long? Transit_Regions_ID { get; set; }

    public long? Export_Regions_ID { get; set; }

    public short? Governate_ID { get; set; }

    public long? ShippingCompanies_ID { get; set; }

    public virtual Country ExportCountry { get; set; } = null!;

    public virtual Im_CheckRequest? Im_CheckRequest { get; set; }

    public virtual ICollection<Im_CheckRequestData_Extra> Im_CheckRequestData_Extras { get; set; } = new List<Im_CheckRequestData_Extra>();

    public virtual InternationalTransportation? InternationalTransportation { get; set; }

    public virtual Shipment_Mean? Shipment_Mean { get; set; }

    public virtual ShippingCompany? ShippingCompanies { get; set; }

    public virtual Transport_Mean? Transport_Mean { get; set; }
}
