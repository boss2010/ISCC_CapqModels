using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class A_AttachmentData_Ex_CommitteeResult_Infection
{
    public long Id { get; set; }

    public long Ex_CommitteeResult_id { get; set; }

    public string? Infection_Comment { get; set; }

    public byte[]? AttachmentPath_Binary { get; set; }

    public short? User_Creation_Id { get; set; }

    public DateTime? User_Creation_Date { get; set; }

    public virtual Ex_CommitteeResult Ex_CommitteeResult { get; set; } = null!;
}
