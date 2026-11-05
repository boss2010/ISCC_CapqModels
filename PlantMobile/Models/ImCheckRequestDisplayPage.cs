using System;
using System.Collections.Generic;

namespace PlantMobile.Models;

public sealed class ImCheckRequestDisplayVm
{
    public long ID { get; set; }
    public string CheckRequest_Number { get; set; } = string.Empty;
    public long Outlet_ID { get; set; }
    public string ExportCompanyAddress { get; set; } = string.Empty;
    public DateTime? User_Creation_Date { get; set; }
    public string Customs_Certificate_Number { get; set; } = string.Empty;
    public DateTime? Certificate_Date { get; set; }
    public string Manifest_Number { get; set; } = string.Empty;
    public DateTime? Shipping_Date { get; set; }
    public DateTime? Arrival_Date { get; set; }
    public string Operation_Type_Name { get; set; } = string.Empty;
    public string Completion_Notes { get; set; } = string.Empty;
    public byte? Completion_Status { get; set; }
    public DateTime? Completion_Request_Date { get; set; }
    public DateTime? Completion_Response_Date { get; set; }
    public long? Im_CheckRequest_Customs_MessageID { get; set; }
    public string Importer_Name { get; set; } = string.Empty;
    public string ExportCountryName { get; set; } = string.Empty;
    public string TransitCountryName { get; set; } = string.Empty;
    public string ArrivalPortType { get; set; } = string.Empty;
    public string ExportPortType { get; set; } = string.Empty;
    public string TransitPortType { get; set; } = string.Empty;
    public string ArrivalPortName { get; set; } = string.Empty;
    public string ExportPortName { get; set; } = string.Empty;
    public string TransitPortName { get; set; } = string.Empty;
    public bool? IsAccepted { get; set; }
    public bool? IsPaid { get; set; }
    public bool? IsActive { get; set; }
    public List<DisplayItemVm> List_Items { get; set; } = new();
    public List<DisplayPermissionVm> List_Permission { get; set; } = new();
    public List<DisplayAttachmentVm> List_Attachments { get; set; } = new();
    public string ErrorMessage { get; set; } = string.Empty;
}

public sealed class DisplayPermissionVm
{
    public long ID { get; set; }
    public string ImPermission_Number { get; set; } = string.Empty;
    public string Item_Name { get; set; } = string.Empty;
    public decimal? GrossWeight { get; set; }
}

public sealed class DisplayItemVm
{
    public long ID { get; set; }
    public long? Item_ShortName_ID { get; set; }
    public string HSCode { get; set; } = string.Empty;
    public string Item_Name { get; set; } = string.Empty;
    public string Scientific_Name { get; set; } = string.Empty;
    public string Short_Name { get; set; } = string.Empty;
    public string Item_Status { get; set; } = string.Empty;
    public string Purpose { get; set; } = string.Empty;
    public string Plant_Part { get; set; } = string.Empty;
    public string Im_InitiatorCounteryName { get; set; } = string.Empty;
    public decimal? GrossWeight { get; set; }
    public decimal? Net_Weight { get; set; }
    public long? Im_Initiator_ID { get; set; }
    public string KingdomName { get; set; } = string.Empty;
    public string PhylumSubphylumName { get; set; } = string.Empty;
    public string OrderName { get; set; } = string.Empty;
    public string ItemTypeName { get; set; } = string.Empty;
    public string MainClassificationName { get; set; } = string.Empty;
    public string SecondaryClassificationName { get; set; } = string.Empty;
    public string FamilyName { get; set; } = string.Empty;
    public string GroupName { get; set; } = string.Empty;
    public List<DisplayLotVm> List_Lots { get; set; } = new();
}

public sealed class DisplayLotVm
{
    public long ID { get; set; }
    public string Lot_Number { get; set; } = string.Empty;
    public string categoryName { get; set; } = string.Empty;
    public string packageType { get; set; } = string.Empty;
    public string packageMaterialName { get; set; } = string.Empty;
    public string DistinctiveMark { get; set; } = string.Empty;
    public string PackagesCount { get; set; } = string.Empty;
    public decimal? GrossWeight { get; set; }
    public decimal? Net_Weight { get; set; }
    public decimal? Based_Weight { get; set; }
    public decimal? Package_Weight { get; set; }
    public decimal? Package_Based_Weight { get; set; }
    public decimal? Package_Net_Weight { get; set; }
    public int? Units_Number { get; set; }
    public string Waybill { get; set; } = string.Empty;
    public string ShipName { get; set; } = string.Empty;
    public string Order_Text { get; set; } = string.Empty;
    public int? containers_type_ID { get; set; }
    public DateTime? TripDate { get; set; }
}

public sealed class DisplayAttachmentVm
{
    public string AttachmentPath { get; set; } = string.Empty;
    public string Attachment_Name { get; set; } = string.Empty;
    public string Attachment_Number { get; set; } = string.Empty;
}

public sealed class AcceptDisplayRequestVm
{
    public long ID { get; set; }
    public bool? IsAccepted { get; set; }
    public string CheckRequest_Number { get; set; } = string.Empty;
    public long Outlet_ID { get; set; }
    public string Completion_Notes { get; set; } = string.Empty;
}

public sealed class ContinueDisplayRequestVm
{
    public long ID { get; set; }
    public string CheckRequest_Number { get; set; } = string.Empty;
    public long Outlet_ID { get; set; }
    public string Completion_Notes { get; set; } = string.Empty;
}
