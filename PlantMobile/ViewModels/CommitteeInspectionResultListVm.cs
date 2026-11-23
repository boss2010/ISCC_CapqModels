namespace ViewModels;

/// <summary>Filters for the committee inspection result grid (نتيجة فحص اللجنة - عرض الكل).</summary>
public sealed class CommitteeInspectionResultFilterVm
{
    public string? RequestNumber { get; set; }
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
    public long? OutletId { get; set; }
    public long? CountryTypeId { get; set; }
    public long? ProcedureId { get; set; }
    public long? InspectionResultId { get; set; }
    public long? ItemId { get; set; }
    public long? OriginCountryId { get; set; }
    public long? InfectionTypeId { get; set; }
    public long? AidTypeId { get; set; }
    public string? Message { get; set; }
    public decimal? QuantityFrom { get; set; }
    public decimal? QuantityTo { get; set; }
    public string? Search { get; set; }

    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 50;
}

/// <summary>Page model for the committee inspection result grid.</summary>
public sealed class CommitteeInspectionResultListPageVm
{
    public CommitteeInspectionResultFilterVm Filter { get; set; } = new();
    public List<CommitteeInspectionResultRowVm> Rows { get; set; } = new();
    public int Total { get; set; }

    public int PageCount => Filter.PageSize <= 0
        ? 1
        : Math.Max(1, (int)Math.Ceiling(Total / (double)Filter.PageSize));

    public List<CommitteeInspectionResultOptionVm> Outlets { get; set; } = new();
    public List<CommitteeInspectionResultOptionVm> CountryTypes { get; set; } = new();
    public List<CommitteeInspectionResultOptionVm> AidTypes { get; set; } = new();
    public List<CommitteeInspectionResultOptionVm> Procedures { get; set; } = new();
    public List<CommitteeInspectionResultOptionVm> InspectionResults { get; set; } = new();
    public List<CommitteeInspectionResultOptionVm> Items { get; set; } = new();
    public List<CommitteeInspectionResultOptionVm> Countries { get; set; } = new();
    public List<CommitteeInspectionResultOptionVm> InfectionTypes { get; set; } = new();
}

/// <summary>One row of the committee inspection result grid.</summary>
public sealed class CommitteeInspectionResultRowVm
{
    public long Id { get; set; }
    public long? Serial { get; set; }
    public string CheckRequestNumber { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string CountryTypeName { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public string ProcedureName { get; set; } = string.Empty;
    public string TransportMeanName { get; set; } = string.Empty;
    public string ItemName { get; set; } = string.Empty;
    public string OriginCountryName { get; set; } = string.Empty;
    public decimal? QuantityTon { get; set; }
    public string InspectionResultName { get; set; } = string.Empty;
    public string InfectionTypeName { get; set; } = string.Empty;
    public string AidTypeName { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public string OutletName { get; set; } = string.Empty;
    public DateTime? DocumentDate { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? CreationDate { get; set; }
}

/// <summary>Columns offered in the print/Excel modal; order is the export order.</summary>
public static class CommitteeInspectionResultExportColumns
{
    public static readonly (string Key, string Header)[] All =
    {
        ("serial", "مسلسل"),
        ("requestNumber", "رقم طلب الفحص"),
        ("message", "الرسالة"),
        ("countryType", "نوع الدولة"),
        ("source", "المصدر"),
        ("procedure", "الإجراء"),
        ("transportMean", "النوع"),
        ("item", "الصنف"),
        ("originCountry", "المنشأ"),
        ("quantityTon", "الكمية بالطن"),
        ("inspectionResult", "نتيجة الفحص"),
        ("infectionType", "نوع الإصابة"),
        ("aidType", "نوع الإعانة"),
        ("notes", "ملاحظات"),
        ("outlet", "المنفذ"),
        ("documentDate", "التاريخ")
    };
}
