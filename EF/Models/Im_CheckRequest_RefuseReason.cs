using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// اسباب رفض الطلب وارد
/// </summary>
public partial class Im_CheckRequest_RefuseReason
{
    public long ID { get; set; }

    public long Im_CheckRequest_Id { get; set; }

    public short Refuse_Reason_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public long User_Creation_Id { get; set; }

    public virtual Im_CheckRequest Im_CheckRequest { get; set; } = null!;

    public virtual Refuse_Reason Refuse_Reason { get; set; } = null!;
}
