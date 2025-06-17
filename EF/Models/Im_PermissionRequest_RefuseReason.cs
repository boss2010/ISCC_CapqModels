using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Im_PermissionRequest_RefuseReason
{
    public long ID { get; set; }

    public long Im_PermissionRequest_Id { get; set; }

    public short Refuse_Reason_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public long User_Creation_Id { get; set; }

    public string? Nots { get; set; }

    public bool? ISActive { get; set; }

    public virtual Im_PermissionRequest Im_PermissionRequest { get; set; } = null!;

    public virtual Refuse_Reason Refuse_Reason { get; set; } = null!;
}
