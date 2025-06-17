using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// الشركة او الهيئة او الفرد
/// </summary>
public partial class Farm_Company
{
    public long ID { get; set; }

    public long? Company_ID { get; set; }

    /// <summary>
    /// from systemcode table 3
    /// </summary>
    public int? ExporterType_Id { get; set; }

    public long? Farm_ID { get; set; }

    public DateOnly? Start_Date { get; set; }

    public DateOnly? End_Date { get; set; }

    public bool IsAcive { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public virtual FarmsDatum? Farm { get; set; }
}
