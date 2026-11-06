namespace PlantMobile.Models;

/// <summary>One import-permission row used by the reprint and stopping screens.</summary>
public sealed class PermissionScreenRow
{
    public long Im_PermissionRequest_ID { get; init; }
    public string ImPermission_Number { get; init; } = string.Empty;
    public DateTime? Arrival_Date { get; init; }
    public bool? IS_Print_Ar { get; init; }
    public bool? IS_Print_EN { get; init; }
    public bool? IsPaid { get; init; }
    public bool? IsAcceppted { get; init; }
    public long? Im_CheckRequest_ID { get; init; }
    public string OperationTypeName { get; init; } = string.Empty;
    public string ExportCountryName { get; init; } = string.Empty;
    public string ImporterTypeName { get; init; } = string.Empty;
    public string ImporterName { get; init; } = string.Empty;
    public string ShortName { get; init; } = string.Empty;
}
