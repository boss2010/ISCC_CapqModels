namespace ViewModels;

public class ListImPermissionFilterVm
{
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public string PermissionNumber { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string ShortName { get; set; } = string.Empty;
    public long? CountryId { get; set; }
    public long? CompanyId { get; set; }
    public long? ShortNameId { get; set; }
    public int Status { get; set; } = 5;
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public int TotalCount { get; set; }
    public int TotalPages => TotalCount == 0 ? 0 : (int)Math.Ceiling(TotalCount / (double)PageSize);
    public int AllCount { get; set; }
    public int ApprovedCount { get; set; }
    public int PendingCount { get; set; }
    public int RejectedCount { get; set; }
    public int UnpaidCount { get; set; }
    public List<ListImPermissionRowVm> Results { get; set; } = new();
}

public class PermissionLookupOptionVm
{
    public long Id { get; set; }
    public string Text { get; set; } = string.Empty;
}

public class ListImPermissionRowVm
{
    public long Id { get; set; }
    public long RowNumber { get; set; }
    public string PermissionNumber { get; set; } = string.Empty;
    public DateTime ArrivalDate { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string OperationType { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string ImporterType { get; set; } = string.Empty;
    public string ShortName { get; set; } = string.Empty;
    public bool? IsAccepted { get; set; }
    public bool? IsPaid { get; set; }
    public bool? IsPrintArabic { get; set; }
    public bool? IsPrintEnglish { get; set; }
    public byte? RenewalStatus { get; set; }
}

public class ImPermissionDetailVm
{
    public string PermissionNumber { get; set; } = string.Empty;
    public long Id { get; set; }
    public long RequestDataId { get; set; }
    public DateTime? ArrivalDate { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string OperationType { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string CountryEn { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string ImporterType { get; set; } = string.Empty;
    public int ImporterTypeId { get; set; }
    public string ImporterName { get; set; } = string.Empty;
    public string ImporterNameEn { get; set; } = string.Empty;
    public string ImporterAddress { get; set; } = string.Empty;
    public string ImporterAddressEn { get; set; } = string.Empty;
    public string DelegateName { get; set; } = string.Empty;
    public string DelegateAddress { get; set; } = string.Empty;
    public string PersonIdNo { get; set; } = string.Empty;
    public string ShipName { get; set; } = string.Empty;
    public string TransportMeanName { get; set; } = string.Empty;
    public string TransportMeanNameEn { get; set; } = string.Empty;
    public string ShipmentMeanName { get; set; } = string.Empty;
    public string ShipmentMeanNameEn { get; set; } = string.Empty;
    public string ArrivePortName { get; set; } = string.Empty;
    public string ArrivePortNameType { get; set; } = string.Empty;
    public string TransportPortName { get; set; } = string.Empty;
    public string TransportPortNameType { get; set; } = string.Empty;
    public bool? IsAccepted { get; set; }
    public bool? IsPaid { get; set; }
    public byte? RenewalStatus { get; set; }
    public List<ImPermissionDetailContactVm> Contacts { get; set; } = new();
    public List<ImPermissionDetailActivityVm> CompanyActivities { get; set; } = new();
    public List<ImPermissionDetailImporterCompanyVm> ImporterCompanies { get; set; } = new();
    public List<ImPermissionDetailAttachmentVm> Attachments { get; set; } = new();
    public List<ImPermissionDetailRefuseReasonVm> RefuseReasons { get; set; } = new();
    public List<ImPermissionDetailItemVm> Items { get; set; } = new();
    public string ErrorMessage { get; set; } = string.Empty;
    public string TraceId { get; set; } = string.Empty;
}

public class ImPermissionDetailItemVm
{
    public long Id { get; set; }
    public string ItemPermissionNumber { get; set; } = string.Empty;
    public string ItemName { get; set; } = string.Empty;
    public string ItemNameEn { get; set; } = string.Empty;
    public string ShortName { get; set; } = string.Empty;
    public string ShortNameEn { get; set; } = string.Empty;
    public string ScientificName { get; set; } = string.Empty;
    public string QualitativeGroup { get; set; } = string.Empty;
    public string OriginCountry { get; set; } = string.Empty;
    public string OriginCountryEn { get; set; } = string.Empty;
    public string SubPartName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Purpose { get; set; } = string.Empty;
    public int? PackageCount { get; set; }
    public decimal? PackageWeight { get; set; }
    public decimal? GrossWeight { get; set; }
    public double? Size { get; set; }
    public int? UnitsNumber { get; set; }
    public string PackageType { get; set; } = string.Empty;
    public string PackageMaterial { get; set; } = string.Empty;
    public string OrderText { get; set; } = string.Empty;
    public bool IsAccepted { get; set; }
    public List<ImPermissionDetailCategoryVm> Categories { get; set; } = new();
    public ImPermissionDetailConstrainVm Constrains { get; set; } = new();
}

public class ImPermissionDetailCategoryVm
{
    public string ItemCatGroup { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public double? Size { get; set; }
    public int? PackageCount { get; set; }
    public decimal? PackageWeight { get; set; }
    public int? UnitsNumber { get; set; }
    public string PackageType { get; set; } = string.Empty;
    public string PackageMaterial { get; set; } = string.Empty;
    public string OrderText { get; set; } = string.Empty;
    public bool? IsRegister { get; set; }
    public string RegisterReason { get; set; } = string.Empty;
    public decimal? GrossWeight { get; set; }
}

public class ImPermissionDetailConstrainVm
{
    public string Country { get; set; } = string.Empty;
    public List<string> Texts { get; set; } = new();
    public List<ImPermissionDetailPortVm> Ports { get; set; } = new();
}

public class ImPermissionDetailPortVm
{
    public string PortType { get; set; } = string.Empty;
    public string PortName { get; set; } = string.Empty;
    public string PortTypeEn { get; set; } = string.Empty;
    public string PortNameEn { get; set; } = string.Empty;
}

public class ImPermissionDetailContactVm
{
    public string TypeName { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}

public class ImPermissionDetailActivityVm
{
    public string ActivityType { get; set; } = string.Empty;
    public string EnrollmentNumber { get; set; } = string.Empty;
    public string EnrollmentType { get; set; } = string.Empty;
    public string EnrollmentStart { get; set; } = string.Empty;
    public string EnrollmentEnd { get; set; } = string.Empty;
}

public class ImPermissionDetailImporterCompanyVm
{
    public string Company { get; set; } = string.Empty;
    public string CompanyEn { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string AddressEn { get; set; } = string.Empty;
}

public class ImPermissionDetailAttachmentVm
{
    public long Id { get; set; }
    public string TypeName { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Path { get; set; } = string.Empty;
}

public class ImPermissionDetailRefuseReasonVm
{
    public string NameAr { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string Note { get; set; } = string.Empty;
}
