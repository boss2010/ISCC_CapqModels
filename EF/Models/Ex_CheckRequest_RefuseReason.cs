using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Ex_CheckRequest_RefuseReason
{
    public long ID { get; set; }

    public long Ex_CheckRequest_Id { get; set; }

    public short Refuse_Reason_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public long User_Creation_Id { get; set; }

    public virtual Ex_CheckRequest Ex_CheckRequest { get; set; } = null!;

    public virtual Refuse_Reason Refuse_Reason { get; set; } = null!;
}
