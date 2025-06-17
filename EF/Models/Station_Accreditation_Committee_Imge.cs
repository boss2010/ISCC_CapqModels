using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Station_Accreditation_Committee_Imge
{
    public long ID { get; set; }

    public long Station_Accreditation_Committee_id { get; set; }

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

    public virtual Station_Accreditation_Committee Station_Accreditation_Committee { get; set; } = null!;
}
