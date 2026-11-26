namespace ViewModels;

public sealed class PermissionOriginChangePageVm
{
    public long PermissionRequestId { get; set; }
    public string PermissionNumber { get; set; } = string.Empty;
    public long? RequestDataId { get; set; }
    public DateTime? ArrivalDate { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string ShipName { get; set; } = string.Empty;
    public string DelegateName { get; set; } = string.Empty;
    public string DelegateAddress { get; set; } = string.Empty;
    public string ExportCountry { get; set; } = string.Empty;
    public string OperationType { get; set; } = string.Empty;
    public string ShipmentMean { get; set; } = string.Empty;
    public string TransportMean { get; set; } = string.Empty;
    public List<PermissionLookupVm> PackageTypes { get; set; } = new();
    public List<PermissionLookupVm> PackageMaterials { get; set; } = new();
    public long? ShippingPortId { get; set; }
    public long? ArrivalPortId { get; set; }
    public List<PermissionLookupVm> ShippingPorts { get; set; } = new();
    public List<PermissionLookupVm> ArrivalPorts { get; set; } = new();
    public short? ExportCountryId { get; set; }
    public int? OperationTypeId { get; set; }
    public short? ShipmentMeanId { get; set; }
    public short? TransportMeanId { get; set; }
    public List<PermissionLookupVm> Countries { get; set; } = new();
    public List<PermissionLookupVm> OperationTypes { get; set; } = new();
    public List<PermissionLookupVm> ShipmentMeans { get; set; } = new();
    public List<PermissionLookupVm> TransportMeans { get; set; } = new();
    public List<PermissionOriginChangeItemVm> Items { get; set; } = new();
    public string ReturnUrl { get; set; } = string.Empty;
}

public sealed class PermissionOriginChangeItemVm
{
    public long PermissionItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string ScientificName { get; set; } = string.Empty;
    public string CurrentCountry { get; set; } = string.Empty;
    public long? SelectedInitiatorId { get; set; }
    public List<PermissionOriginOptionVm> Origins { get; set; } = new();
    public string ItemPermissionNumber { get; set; } = string.Empty;
    public short? PackageTypeId { get; set; }
    public short? PackageMaterialId { get; set; }
    public string OrderText { get; set; } = string.Empty;
    public int? PackageCount { get; set; }
    public decimal? PackageWeight { get; set; }
    public int? UnitsNumber { get; set; }
    public double? Size { get; set; }
    public decimal? GrossWeight { get; set; }
}
public sealed class PermissionLookupVm { public long Id { get; set; } public string Name { get; set; }=string.Empty; }

public sealed class PermissionOriginOptionVm
{
    public long InitiatorId { get; set; }
    public short CountryId { get; set; }
    public string CountryName { get; set; } = string.Empty;
}

public sealed class PermissionOriginChangeSaveVm
{
    public long PermissionRequestId { get; set; }
    public string PermissionNumber { get; set; } = string.Empty;
    public string ReturnUrl { get; set; } = string.Empty;
    public DateTime? ArrivalDate { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string ShipName { get; set; } = string.Empty;
    public string DelegateName { get; set; } = string.Empty;
    public string DelegateAddress { get; set; } = string.Empty;
    public long? ShippingPortId { get; set; }
    public long? ArrivalPortId { get; set; }
    public short? ExportCountryId { get; set; }
    public int? OperationTypeId { get; set; }
    public short? ShipmentMeanId { get; set; }
    public short? TransportMeanId { get; set; }
    public List<PermissionOriginChangeSaveItemVm> Items { get; set; } = new();
}

public sealed class PermissionOriginChangeSaveItemVm
{
    public long PermissionItemId { get; set; }
    public long InitiatorId { get; set; }
    public string ItemPermissionNumber { get; set; } = string.Empty;
    public short? PackageTypeId { get; set; }
    public short? PackageMaterialId { get; set; }
    public string OrderText { get; set; } = string.Empty;
}
