using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class EX
{
    public string CheckRequest_Number { get; set; } = null!;

    public long Ex_CheckRequest_ID { get; set; }

    public long? Ex_CheckRequest_Items_ID { get; set; }

    public byte? CommitteeType_ID { get; set; }

    public long Committee_ID { get; set; }

    public long? LotData_ID { get; set; }

    public DateTime? Date { get; set; }

    public bool? IsAdminResult { get; set; }

    public double? QuantitySize { get; set; }

    public double? Weight { get; set; }

    public string? Notes { get; set; }

    public bool? IS_Total { get; set; }

    public long? Item_ShortName_ID { get; set; }

    public DateOnly? Delegation_Date { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public bool? IsFinishedAll { get; set; }

    public bool? Status { get; set; }

    public string? ShortName_Ar { get; set; }

    public string? ShortName_En { get; set; }

    public long? Expr1 { get; set; }

    public long Ex_CommitteeResult_ID { get; set; }

    public DateTime Expr2 { get; set; }

    public long EmployeeId { get; set; }

    public string? Expr3 { get; set; }

    public bool IsAccepted { get; set; }
}
