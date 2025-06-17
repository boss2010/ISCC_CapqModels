using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// صور فحص الاصابه
/// </summary>
public partial class A_AttachmentData_Im_CommitteeResult_Infection
{
    public long Id { get; set; }

    public long Im_CommitteeResult_id { get; set; }

    /// <summary>
    /// نوع المرفق
    /// </summary>
    public string? Infection_Comment { get; set; }

    public byte[]? AttachmentPath_Binary { get; set; }

    /// <summary>
    /// null-&gt; for user , value -&gt; if the admin add the row
    /// </summary>
    public short? User_Creation_Id { get; set; }

    /// <summary>
    /// null-&gt; for user , value -&gt; if the admin add the row
    /// </summary>
    public DateTime? User_Creation_Date { get; set; }

    public virtual Im_CommitteeResult Im_CommitteeResult { get; set; } = null!;
}
