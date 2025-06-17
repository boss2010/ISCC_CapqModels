using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Ex_Request_TreatmentData_Confirm
{
    public long ID { get; set; }

    public long Ex_Request_TreatmentData_ID { get; set; }

    public DateTime Date { get; set; }

    public long EmployeeId { get; set; }

    public string? Notes { get; set; }

    public bool IsAccepted { get; set; }

    public virtual Ex_Request_TreatmentDatum Ex_Request_TreatmentData { get; set; } = null!;
}
