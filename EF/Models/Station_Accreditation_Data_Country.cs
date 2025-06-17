using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// بلد نشاط المحطة
/// </summary>
public partial class Station_Accreditation_Data_Country
{
    public long ID { get; set; }

    public long Station_Accreditation_Data_ID { get; set; }

    public short CountryID { get; set; }

    public bool IsActive { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public virtual Country Country { get; set; } = null!;

    public virtual Station_Accreditation_Datum Station_Accreditation_Data { get; set; } = null!;
}
