using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Ex_CheckRequest_SampleData_Confirm
{
    public long ID { get; set; }

    public long Ex_CheckRequest_SampleData_ID { get; set; }

    public DateTime Date { get; set; }

    public short EmployeeId { get; set; }

    public string? Notes { get; set; }

    public bool IsAccepted { get; set; }

    public virtual Ex_CheckRequest_SampleDatum Ex_CheckRequest_SampleData { get; set; } = null!;
}
