namespace ViewModels;

/// <summary>Page model for "نتيجة فحص اللجنة" (Committee inspection result).</summary>
public sealed class CommitteeInspectionResultPageVm
{
    public string OutletName { get; set; } = "المكتب";
    public string Message { get; set; } = "وارد";
    public DateTime DocumentDate { get; set; } = DateTime.Now;
    public string RequestNumber { get; set; } = string.Empty;

    public List<CommitteeInspectionResultOptionVm> Procedures { get; set; } = new();
    public List<CommitteeInspectionResultOptionVm> CountryTypes { get; set; } = new();
    public List<CommitteeInspectionResultOptionVm> AidTypes { get; set; } = new();
    public List<CommitteeInspectionResultOptionVm> TransportMeans { get; set; } = new();
    public List<CommitteeInspectionResultOptionVm> Items { get; set; } = new();
    public List<CommitteeInspectionResultOptionVm> Countries { get; set; } = new();
    public List<CommitteeInspectionResultOptionVm> InspectionResults { get; set; } = new();
    public List<CommitteeInspectionResultOptionVm> InfectionTypes { get; set; } = new();
    public List<CommitteeInspectionResultOptionVm> OperationTypes { get; set; } = new();
    public List<CommitteeInspectionResultCompanyVm> Companies { get; set; } = new();
    public List<CommitteeInspectionResultOptionVm> Requests { get; set; } = new();
}

public sealed class CommitteeInspectionResultOptionVm
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public sealed class CommitteeInspectionResultSaveVm
{
    public long Serial { get; set; }
    public string RequestNumber { get; set; } = string.Empty;
    public string Message { get; set; } = "وارد";
    /// <summary>Combined importer key: "ImporterType_Id:Importer_ID" (e.g. "6:2646").</summary>
    public string? Company { get; set; }
    public long? CountryTypeId { get; set; }
    public string Source { get; set; } = string.Empty;
    public long? ProcedureId { get; set; }
    public long? TransportMeanId { get; set; }
    public long? ItemId { get; set; }
    public long? OriginCountryId { get; set; }
    public decimal? QuantityTon { get; set; }
    public long? InspectionResultId { get; set; }
    public long? InfectionTypeId { get; set; }
    public long? AidTypeId { get; set; }
    public string Notes { get; set; } = string.Empty;
}

/// <summary>Local copies of the DTO enums; the mobile project does not reference PlantQuar.DTO.</summary>
public static class CommitteeInspectionResultDefaults
{
    public static readonly List<CommitteeInspectionResultOptionVm> CountryTypes = new()
    {
        new() { Id = 1, Name = "عربية" },
        new() { Id = 2, Name = "أفريقية (غير عربية)" },
        new() { Id = 3, Name = "آسيوية (غير عربية)" },
        new() { Id = 4, Name = "أوروبية" },
        new() { Id = 5, Name = "أمريكية" },
        new() { Id = 6, Name = "أوقيانوسية" },
        new() { Id = 7, Name = "أخرى" }
    };

    public static readonly List<CommitteeInspectionResultOptionVm> AidTypes = new()
    {
        new() { Id = 0, Name = "بدون" },
        new() { Id = 1, Name = "باعانة" }
    };
}
