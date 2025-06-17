using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class View_List_Im_PermissionRequest
{
    public long Im_PermissionRequest_ID { get; set; }

    public decimal? ImPermission_Number { get; set; }

    public DateOnly Arrival_Date { get; set; }

    public string? operationTypeName { get; set; }

    public string? ExportCountryName { get; set; }

    public long Importer_ID { get; set; }

    public int ImporterType_Id { get; set; }

    public bool? IsAcceppted { get; set; }

    public bool? IsPaid { get; set; }

    public bool? IS_Print_Ar { get; set; }

    public bool? IS_Print_EN { get; set; }

    public long? Im_CheckRequest_ID { get; set; }

    public DateOnly? Start_Date { get; set; }

    public DateOnly? End_Date { get; set; }

    public byte? Renewal_Status { get; set; }

    public byte? Print_Count { get; set; }

    public string? ImporterTypeName { get; set; }

    public string? ImporterName { get; set; }

    public string? shortName { get; set; }
}
