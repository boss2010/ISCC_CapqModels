using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Farm_SampleData_Confirm
{
    public long ID { get; set; }

    public long Farm_SampleData_ID { get; set; }

    public DateTime Date { get; set; }

    public short EmployeeId { get; set; }

    public string? Notes { get; set; }

    public bool IsAccepted { get; set; }

    public virtual Farm_SampleDatum Farm_SampleData { get; set; } = null!;
}
