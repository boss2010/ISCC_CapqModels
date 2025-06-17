using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Station_Accreditation
{
    public long ID { get; set; }

    /// <summary>
    /// المحطة
    /// </summary>
    public long Station_ID { get; set; }

    /// <summary>
    /// المحطة
    /// </summary>
    public long Station_Accreditation_Data_ID { get; set; }

    public long? Station_Accreditation_Request_ID { get; set; }

    public string? Notes_Quarantine { get; set; }

    /// <summary>
    /// تاريخ البداية
    /// </summary>
    public DateOnly? StartDate { get; set; }

    /// <summary>
    /// تاريخ النهاية
    /// </summary>
    public DateOnly? EndDate { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public virtual Station Station { get; set; } = null!;

    public virtual ICollection<StationCompany> StationCompanies { get; set; } = new List<StationCompany>();

    public virtual Station_Accreditation_Datum Station_Accreditation_Data { get; set; } = null!;
}
