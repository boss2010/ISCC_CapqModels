using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// وسائل اتصال المحطة
/// </summary>
public partial class StationContact
{
    public int ID { get; set; }

    public long StationID { get; set; }

    /// <summary>
    /// نوع وسيلة الاتصال
    /// </summary>
    public byte ContactType_ID { get; set; }

    /// <summary>
    /// الرقم
    /// </summary>
    public string Value { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short? User_Creation_Id { get; set; }

    public DateTime? User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public virtual ContactType ContactType { get; set; } = null!;

    public virtual Station Station { get; set; } = null!;
}
