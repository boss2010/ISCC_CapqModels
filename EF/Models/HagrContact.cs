using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class HagrContact
{
    public int ID { get; set; }

    public long ContactOwnerID { get; set; }

    /// <summary>
    /// from systemcode table 5
    /// </summary>
    public int OutlitAdmin { get; set; }

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

    public virtual A_SystemCode OutlitAdminNavigation { get; set; } = null!;
}
