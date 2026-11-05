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
    public DateTime? ArrivalDate { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string OperationType { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string ImporterType { get; set; } = string.Empty;
    public bool? IsAccepted { get; set; }
    public bool? IsPaid { get; set; }
    public byte? RenewalStatus { get; set; }
    public List<ImPermissionDetailItemVm> Items { get; set; } = new();
    public string ErrorMessage { get; set; } = string.Empty;
    public string TraceId { get; set; } = string.Empty;
}

public class ImPermissionDetailItemVm
{
    public long Id { get; set; }
    public string ItemPermissionNumber { get; set; } = string.Empty;
    public string ItemName { get; set; } = string.Empty;
    public string ShortName { get; set; } = string.Empty;
    public string OriginCountry { get; set; } = string.Empty;
    public int? PackageCount { get; set; }
    public decimal? PackageWeight { get; set; }
    public decimal? GrossWeight { get; set; }
    public double? Size { get; set; }
    public bool IsAccepted { get; set; }
}
