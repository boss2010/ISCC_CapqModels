using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Farm_Country
{
    public long ID { get; set; }

    public long Farm_Request_ID { get; set; }

    /// <summary>
    /// ConstrainOwner(UnionId/CountryId/ or 0 if Local-Egypt)
    /// </summary>
    public short? Country_ID { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    /// <summary>
    /// الحجر واقف على الدولة ولا لا
    /// </summary>
    public bool? IsAcceppted { get; set; }

    public DateOnly? End_Date { get; set; }

    public DateOnly? Start_Date { get; set; }

    public bool? IsActive { get; set; }

    public virtual Country? Country { get; set; }

    public virtual Farm_Request Farm_Request { get; set; } = null!;
}
