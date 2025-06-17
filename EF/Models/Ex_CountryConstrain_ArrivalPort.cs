using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// موانى تحديد ميناء وصول معين
/// </summary>
public partial class Ex_CountryConstrain_ArrivalPort
{
    public long Id { get; set; }

    public long Ex_CountryConstrain_Id { get; set; }

    public int Port_International_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public long User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public long? User_Deletion_Id { get; set; }

    public long? Parent_ID { get; set; }

    public bool? IsActive { get; set; }

    public virtual Ex_CountryConstrain Ex_CountryConstrain { get; set; } = null!;

    public virtual Port_International Port_International { get; set; } = null!;
}
