using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// نتيجه سحب عينه للمساعد
/// </summary>
public partial class Im_CheckRequest_SampleData_Confirm
{
    public long ID { get; set; }

    public long Im_CheckRequest_SampleData_ID { get; set; }

    public DateTime Date { get; set; }

    public short EmployeeId { get; set; }

    public string? Notes { get; set; }

    public bool IsAccepted { get; set; }

    public virtual Im_CheckRequest_SampleDatum Im_CheckRequest_SampleData { get; set; } = null!;
}
