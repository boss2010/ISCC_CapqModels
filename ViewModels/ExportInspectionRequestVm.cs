namespace ViewModels;

public class ExportInspectionRequestListVm
{
    public string RequestNumber { get; set; } = string.Empty;
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public int Status { get; set; }
    public long? StationId { get; set; }
    public string StationName { get; set; } = string.Empty;
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 15;
    public int TotalCount { get; set; }
    public int TotalPages => Math.Max(1, (int)Math.Ceiling(TotalCount / (double)PageSize));
    public List<ExportInspectionRequestRowVm> Results { get; set; } = [];
}

public class ExportStationDashboardVm
{
    public string Search { get; set; } = string.Empty;
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public int TotalCount { get; set; }
    public int TotalPages => Math.Max(1,(int)Math.Ceiling(TotalCount/(double)PageSize));
    public List<ExportStationDashboardRowVm> Stations { get; set; } = [];
}

public class ExportStationDashboardRowVm
{
    public long RowNumber { get; set; }
    public long StationId { get; set; }
    public string StationName { get; set; } = string.Empty;
    public string StationCode { get; set; } = string.Empty;
    public string Governorate { get; set; } = string.Empty;
    public int RequestCount { get; set; }
    public int CommitteeCount { get; set; }
    public int InspectionCount { get; set; }
    public int SamplingCount { get; set; }
    public int TreatmentCount { get; set; }
    public int CurrentEngineerCount { get; set; }
}

public class ExportInspectionRequestRowVm
{
    public long RowNumber { get; set; }
    public long Id { get; set; }
    public string RequestNumber { get; set; } = string.Empty;
    public DateTime? CreationDate { get; set; }
    public string Country { get; set; } = string.Empty;
    public string ImporterType { get; set; } = string.Empty;
    public string ImporterName { get; set; } = string.Empty;
    public string ExaminationPlace { get; set; } = string.Empty;
    public string GenshiPlace { get; set; } = string.Empty;
    public bool? IsAccepted { get; set; }
    public bool? IsPaid { get; set; }
    public int ClosedRequest { get; set; }
    public int FinalResultId { get; set; }
    public string FinalResultName { get; set; } = string.Empty;
}

public class ExportInspectionRequestDetailsVm
{
    public long Id { get; set; }
    public string RequestNumber { get; set; } = string.Empty;
    public DateTime? CreationDate { get; set; }
    public string ExportCompany { get; set; } = string.Empty;
    public string ExportCompanyAddress { get; set; } = string.Empty;
    public string NationalId { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string ImporterType { get; set; } = string.Empty;
    public string ImporterName { get; set; } = string.Empty;
    public string OwnerName { get; set; } = string.Empty;
    public string OwnerAddress { get; set; } = string.Empty;
    public string DelegateName { get; set; } = string.Empty;
    public string DelegateAddress { get; set; } = string.Empty;
    public string ShipName { get; set; } = string.Empty;
    public string ShipmentMean { get; set; } = string.Empty;
    public string TransportMean { get; set; } = string.Empty;
    public string ImportCompany { get; set; } = string.Empty;
    public string ImportCompanyAddress { get; set; } = string.Empty;
    public string ReceiverName { get; set; } = string.Empty;
    public string ShippingPort { get; set; } = string.Empty;
    public string ArrivalPortType { get; set; } = string.Empty;
    public string ArrivalPort { get; set; } = string.Empty;
    public string TransitCountry { get; set; } = string.Empty;
    public string TransitPortType { get; set; } = string.Empty;
    public string TransitPort { get; set; } = string.Empty;
    public string ExaminationGovernorate { get; set; } = string.Empty;
    public string ExaminationCenter { get; set; } = string.Empty;
    public string ExaminationAddress { get; set; } = string.Empty;
    public long? ExaminationStationId { get; set; }
    public string ExaminationStationName { get; set; } = string.Empty;
    public long? ExaminationOutletId { get; set; }
    public string ExaminationPlace { get; set; } = string.Empty;
    public string GenshiPlace { get; set; } = string.Empty;
    public bool? IsAccepted { get; set; }
    public bool? IsPaid { get; set; }
    public int ClosedRequest { get; set; }
    public string FinalResult { get; set; } = string.Empty;
    public DateTime? FinalResultDate { get; set; }
    public string RejectNotes { get; set; } = string.Empty;
    public decimal? Amount { get; set; }
    public List<ExportInspectionItemVm> Items { get; set; } = [];
    public List<ExportInspectionAttachmentVm> Attachments { get; set; } = [];
    public List<ExportInspectionFeeVm> Fees { get; set; } = [];
    public List<ExportStationEngineerVm> StationEngineers { get; set; } = [];
    public List<ExportRequestCommitteeVm> Committees { get; set; } = [];
}

public class ExportInspectionItemVm
{
    public long Id { get; set; }
    public string ShortName { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string CategoryGroup { get; set; } = string.Empty;
    public string FarmCode { get; set; } = string.Empty;
    public string FarmName { get; set; } = string.Empty;
    public decimal? GrossWeight { get; set; }
    public decimal? NetWeight { get; set; }
    public int? PackageCount { get; set; }
    public string PackageType { get; set; } = string.Empty;
    public string PackageMaterial { get; set; } = string.Empty;
    public bool IsAccepted { get; set; }
    public List<ExportInspectionLotVm> Lots { get; set; } = [];
}

public class ExportInspectionLotVm
{
    public long Id { get; set; }
    public string LotNumber { get; set; } = string.Empty;
    public int? PackageCount { get; set; }
    public decimal? GrossWeight { get; set; }
    public decimal? NetWeight { get; set; }
    public decimal? PackageGrossWeight { get; set; }
    public decimal? PackageNetWeight { get; set; }
    public string PackageType { get; set; } = string.Empty;
    public string PackageMaterial { get; set; } = string.Empty;
    public string GrowerNumber { get; set; } = string.Empty;
    public string Waybill { get; set; } = string.Empty;
    public bool? IsAccepted { get; set; }
    public string RejectReason { get; set; } = string.Empty;
}

public class ExportInspectionAttachmentVm
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public class ExportInspectionFeeVm
{
    public decimal? TotalGrossWeight { get; set; }
    public decimal? FeeValue { get; set; }
    public decimal? TotalAmount { get; set; }
    public bool? IsRounded { get; set; }
}

public class ExportStationEngineerVm
{
    public long UserId { get; set; }
    public long? EmployeeId { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class ExportRequestCommitteeVm
{
    public long Id { get; set; }
    public byte CommitteeTypeId { get; set; }
    public string CommitteeType { get; set; } = string.Empty;
    public DateTime? DelegationDate { get; set; }
    public string Employees { get; set; } = string.Empty;
    public int LotCount { get; set; }
}
