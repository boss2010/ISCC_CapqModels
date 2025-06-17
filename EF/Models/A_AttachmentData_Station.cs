using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// اسم الجدول
/// </summary>
public partial class A_AttachmentData_Station
{
    public long Id { get; set; }

    /// <summary>
    /// نوع المرفق
    /// </summary>
    public short? A_AttachmentTableType_ID { get; set; }

    /// <summary>
    /// اسم الجدول
    /// </summary>
    public short A_AttachmentTableNameId { get; set; }

    /// <summary>
    /// الرقم داخل الجدول
    /// </summary>
    public long RowId { get; set; }

    /// <summary>
    /// رقم المرفق
    /// </summary>
    public string? Attachment_Number { get; set; }

    /// <summary>
    /// اسم المرفق
    /// </summary>
    public string? Attachment_TypeName { get; set; }

    /// <summary>
    /// مسار المرفق
    /// </summary>
    public string? AttachmentPath { get; set; }

    /// <summary>
    /// تاريخ البداية
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// تاريخ النهاية
    /// </summary>
    public DateTime? EndDate { get; set; }

    public byte[]? AttachmentPath_Binary { get; set; }

    /// <summary>
    /// null-&gt; for user , value -&gt; if the admin add the row
    /// </summary>
    public short? User_Creation_Id { get; set; }

    /// <summary>
    /// null-&gt; for user , value -&gt; if the admin add the row
    /// </summary>
    public DateTime? User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public virtual A_AttachmentTableName A_AttachmentTableName { get; set; } = null!;

    public virtual A_AttachmentTableType? A_AttachmentTableType { get; set; }
}
