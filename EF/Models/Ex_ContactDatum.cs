using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// وسائل اتصال المصدر
/// </summary>
public partial class Ex_ContactDatum
{
    public long ID { get; set; }

    /// <summary>
    /// المصدر(شركة أو هيئه عامة)
    /// </summary>
    public long Exporter_ID { get; set; }

    /// <summary>
    /// نوع وسيلة الاتصال
    /// </summary>
    public byte ContactType_ID { get; set; }

    /// <summary>
    /// 0 if National company/ 1 if Public Organization
    /// </summary>
    public int ExporterType_Id { get; set; }

    /// <summary>
    /// الرقم
    /// </summary>
    public string Value { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short? User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public virtual ContactType ContactType { get; set; } = null!;

    public virtual A_SystemCode ExporterType { get; set; } = null!;
}
