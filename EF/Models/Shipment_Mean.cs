using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// وسائل الشحن
/// </summary>
public partial class Shipment_Mean
{
    public byte ID { get; set; }

    /// <summary>
    /// الاسم بالعربية
    /// </summary>
    public string? Ar_Name { get; set; }

    /// <summary>
    /// الاسم بالانجليزية
    /// </summary>
    public string? En_Name { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short? User_Creation_Id { get; set; }

    public DateTime? User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<Ex_CheckRequest_Datum> Ex_CheckRequest_Data { get; set; } = new List<Ex_CheckRequest_Datum>();

    public virtual ICollection<Im_CheckRequest_Datum> Im_CheckRequest_Data { get; set; } = new List<Im_CheckRequest_Datum>();

    public virtual ICollection<Im_RequestDatum> Im_RequestData { get; set; } = new List<Im_RequestDatum>();

    public virtual ICollection<Im_ScientificResearch> Im_ScientificResearches { get; set; } = new List<Im_ScientificResearch>();
}
