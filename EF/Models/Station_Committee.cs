using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Station_Committee
{
    public long Station_Accreditation_Committee_ID { get; set; }

    public long Station_Accreditation_Request_ID { get; set; }

    public DateOnly? Delegation_Date { get; set; }

    public TimeOnly? StartTime { get; set; }

    public TimeOnly? EndTime { get; set; }

    public bool? IsApproved { get; set; }

    public bool? IsPaid { get; set; }

    public bool? Status { get; set; }

    public bool? IsAccepted { get; set; }

    public bool? Is_Start_Android { get; set; }

    public bool? Is_Cancel { get; set; }

    public short? Committee_User_Deletion_Id { get; set; }
}
