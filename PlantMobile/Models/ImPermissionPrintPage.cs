using System;
using System.Collections.Generic;

namespace PlantMobile.Models;

public sealed class ImPermissionPrintDetailsVm
{
    public long? Im_RequestData_ID { get; set; }
    public long Im_PermissionRequest_ID { get; set; }
    public string ImPermission_Number { get; set; } = string.Empty;
    public bool? IS_Print_Ar { get; set; }
    public bool? IS_Print_EN { get; set; }
    public string UserName { get; set; } = "";
    public string UserNameEn { get; set; } = "";
    public string outletName { get; set; } = "";
    public string outletName_En { get; set; } = "";
    public string? delegateName { get; set; }
    public string? delegateAddress { get; set; }
    public string? personIdNo { get; set; }
    public short? User_Creation_Id { get; set; }
    public DateTime? User_Creation_Date { get; set; }
    public string ImporterName { get; set; } = "";
    public string ImporterAddress { get; set; } = "";
    public string ImporterNameEn { get; set; } = "";
    public string ImporterAddressEn { get; set; } = "";
    public string companyActivityType { get; set; } = "";
    public string companyActivityTypeEn { get; set; } = "";
    public string ImporterType { get; set; } = "";
    public string ImporterTypeEn { get; set; } = "";
    public long Importer_ID { get; set; }
    public long? Company_ID { get; set; }
    public int ImporterType_Id { get; set; }
    public string? Enrollment_Name { get; set; }
    public decimal? Enrollment_Number { get; set; }
    public DateTime? Enrollment_Start { get; set; }
    public DateTime? Enrollment_End { get; set; }
    public string? Enrollment_type_AR { get; set; }
    public string? Enrollment_type_EN { get; set; }
    public byte? Shipment_Mean_Id { get; set; }
    public byte? Transport_Mean_Id { get; set; }
    public string Transport_MeanName { get; set; } = "";
    public string Shipment_MeanName { get; set; } = "";
    public string ArrivePortName { get; set; } = "";
    public string TransportPortName { get; set; } = "";
    public string TransportPortNameType { get; set; } = "";
    public string ArrivePortNameType { get; set; } = "";
    public string Ship_Name { get; set; } = "";
    public string ExportCountryName { get; set; } = "";
    public string ExportCountryNameEn { get; set; } = "";
    public string Transport_MeanNameEn { get; set; } = "";
    public string Shipment_MeanNameEn { get; set; } = "";
    public string ArrivePortNameEn { get; set; } = "";
    public string TransportPortNameEn { get; set; } = "";
    public string? Ship_NameEn { get; set; }
    public List<PrintImportersVm> ImportersCompanies { get; set; } = new();
    public List<PrintItemVm> Items { get; set; } = new();
    public bool? IsAcceppted { get; set; }
    public bool? CanPrint { get; set; }
    public bool? IsPaid { get; set; }
    public bool? IsActive { get; set; }
    public byte? Renewal_Status { get; set; }
    public byte? Print_Count { get; set; }
    public DateTime? Start_Date { get; set; }
    public DateTime? End_Date { get; set; }
    public string ErrorMessage { get; set; } = "";
}

public sealed class PrintImportersVm
{
    public string ImporterCompany { get; set; } = "";
    public string ImporterCompanyAddress { get; set; } = "";
    public string ImporterCompanyEn { get; set; } = "";
    public string ImporterCompanyAddressEn { get; set; } = "";
}

public sealed class PrintItemVm
{
    public long? Im_Initiator_ID { get; set; }
    public long? ImPermissionItem_ID { get; set; }
    public string qualitiveGroupName { get; set; } = "";
    public string qualitiveGroupNameEn { get; set; } = "";
    public string Item_Permission_Number { get; set; } = "";
    public string ItemNameAr { get; set; } = "";
    public string ItemNameEn { get; set; } = "";
    public string ItemShortNameAr { get; set; } = "";
    public string ItemShortNameEn { get; set; } = "";
    public string ScientificNameAr { get; set; } = "";
    public string ScientificNameEn { get; set; } = "";
    public string Status { get; set; } = "";
    public string Purpose { get; set; } = "";
    public string StatusEn { get; set; } = "";
    public string PurposeEn { get; set; } = "";
    public string subPartName { get; set; } = "";
    public string subPartNameEn { get; set; } = "";
    public int? SubPart_id { get; set; }
    public string InitiatorCountry { get; set; } = "";
    public string InitiatorCountryEn { get; set; } = "";
    public List<PrintCategoryVm> ItemCategories { get; set; } = new();
    public PrintConstrainVm? Itemconstrains { get; set; }
    public double? Size { get; set; }
    public int? Package_Count { get; set; }
    public decimal? Package_Weight { get; set; }
    public int? Units_Number { get; set; }
    public string packageType { get; set; } = "";
    public string packageTypeEn { get; set; } = "";
    public string packageMaterial { get; set; } = "";
    public string packageMaterialEn { get; set; } = "";
    public short? packageMaterialID { get; set; }
    public short? packageTypeID { get; set; }
    public decimal? GrossWeight { get; set; }
    public string Order_Text { get; set; } = "";
}

public sealed class PrintCategoryVm
{
    public string packageMaterial { get; set; } = "";
    public short? packageMaterialID { get; set; }
    public long? Im_PermissionItems_ID { get; set; }
    public long? Im_PermissionItemsCategory_ID { get; set; }
    public string categoryName { get; set; } = "";
    public string categoryNameEn { get; set; } = "";
    public double? Size { get; set; }
    public int? Package_Count { get; set; }
    public decimal? Package_Weight { get; set; }
    public int? Units_Number { get; set; }
    public string packageType { get; set; } = "";
    public short? packageTypeID { get; set; }
    public decimal? GrossWeight { get; set; }
    public string Order_Text { get; set; } = "";
    public string RegisterReason { get; set; } = "";
    public bool? IsRegister { get; set; }
    public long? ItemCategory_ID { get; set; }
    public string itemCatGroup { get; set; } = "";
}

public sealed class PrintConstrainVm
{
    public List<string> texts_Ar { get; set; } = new();
    public List<string> text_En { get; set; } = new();
    public List<string> InSide_Certificate_Ar { get; set; } = new();
    public List<string> InSide_Certificate_En { get; set; } = new();
    public List<PrintPortVm> itemConstrainPorts { get; set; } = new();
    public string govNameAR { get; set; } = "";
    public string govNameEN { get; set; } = "";
    public string conCountry { get; set; } = "";
}

public sealed class PrintPortVm
{
    public string portType { get; set; } = "";
    public string portName { get; set; } = "";
    public string portTypeEn { get; set; } = "";
    public string portNameEn { get; set; } = "";
    public short? portTypeId { get; set; }
    public int? portId { get; set; }
}
