namespace ViewModels;

public sealed class ExportCommitteeWorkVm
{
    public long CommitteeId { get; set; }
    public long RequestId { get; set; }
    public string RequestNumber { get; set; } = string.Empty;
    public byte CommitteeTypeId { get; set; }
    public string CommitteeType { get; set; } = string.Empty;
    public bool IsAdmin { get; set; }
    public bool IsFinished { get; set; }
    public List<ExportCommitteeResultTypeVm> ResultTypes { get; set; }=[];
    public List<ExportCommitteeWorkRowVm> Rows { get; set; }=[];
}

public sealed class ExportCommitteeResultTypeVm { public byte Id { get; set; } public string Name { get; set; }=string.Empty; }

public sealed class ExportCommitteeWorkRowVm
{
    public long Id { get; set; }
    public long ItemId { get; set; }
    public long LotId { get; set; }
    public string ItemName { get; set; }=string.Empty;
    public string LotNumber { get; set; }=string.Empty;
    public int? PackageCount { get; set; }
    public decimal? OriginalWeight { get; set; }
    public byte? ResultTypeId { get; set; }
    public decimal? Quantity { get; set; }
    public decimal? Weight { get; set; }
    public string Notes { get; set; }=string.Empty;
    public DateTime? WithdrawDate { get; set; }
    public string Barcode { get; set; }=string.Empty;
    public double? SampleSize { get; set; }
    public double? SampleRatio { get; set; }
    public bool? IsAccepted { get; set; }
    public string AnalysisName { get; set; }=string.Empty;
    public string TreatmentMethod { get; set; }=string.Empty;
    public decimal? TreatmentSize { get; set; }
    public decimal? MaterialAmount { get; set; }
    public decimal? Dose { get; set; }
    public int? ExposureMinute { get; set; }
    public int? ExposureHour { get; set; }
    public int? ExposureDay { get; set; }
    public decimal? Temperature { get; set; }
    public string Procedures { get; set; }=string.Empty;
}

