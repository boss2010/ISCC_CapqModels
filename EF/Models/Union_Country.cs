using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// الاتحادات الدولية
/// </summary>
public partial class Union_Country
{
    public short ID { get; set; }

    /// <summary>
    /// الدولة
    /// </summary>
    public short Country_ID { get; set; }

    /// <summary>
    /// الاتحاد
    /// </summary>
    public short Union_ID { get; set; }

    public bool IsActive { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public virtual Country Country { get; set; } = null!;

    public virtual Union Union { get; set; } = null!;
}
