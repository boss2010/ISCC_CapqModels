using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// المواني الدوليه
/// </summary>
public partial class Port_International
{
    public int ID { get; set; }

    /// <summary>
    /// الدولة
    /// </summary>
    public short? Country_ID { get; set; }

    /// <summary>
    /// نوع الميناء
    /// </summary>
    public byte? PortTypeID { get; set; }

    /// <summary>
    /// الاسم بالعربية
    /// </summary>
    public string? Name_Ar { get; set; }

    /// <summary>
    /// الاسم بالانجليزية
    /// </summary>
    public string? Name_En { get; set; }

    public bool IsActive { get; set; }

    public string? Phone { get; set; }

    public string? Fax { get; set; }

    public string? Email { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public long? Regions_ID { get; set; }

    public virtual Country? Country { get; set; }

    public virtual ICollection<Ex_CountryConstrain_ArrivalPort> Ex_CountryConstrain_ArrivalPorts { get; set; } = new List<Ex_CountryConstrain_ArrivalPort>();

    public virtual Port_Type? PortType { get; set; }

    public virtual Region? Regions { get; set; }
}
