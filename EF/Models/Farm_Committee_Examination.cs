using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// معاينة المزرعة
/// </summary>
public partial class Farm_Committee_Examination
{
    public long ID { get; set; }

    /// <summary>
    /// لجنة المعالجة
    /// </summary>
    public long FarmCommittee_ID { get; set; }

    public long? Farm_Request_ItemCategories_ID { get; set; }

    public string? Notes { get; set; }

    /// <summary>
    /// تاريخ نهاية معاينة الصنف
    /// </summary>
    public DateOnly? EndDate { get; set; }

    /// <summary>
    /// تاريخ بداية معاينة الصنف
    /// </summary>
    public DateOnly? StartDate { get; set; }

    /// <summary>
    /// الانتاجية
    /// </summary>
    public double? Quantity_Ton { get; set; }

    /// <summary>
    /// 0 if rejected else 1
    /// </summary>
    public bool? IsAccepted { get; set; }

    public short User_Creation_Id { get; set; }

    /// <summary>
    /// null-&gt;exporter not take action 0 if exporter doesn&apos;t accept else 1
    /// </summary>
    public bool? IsAdminFinalResult { get; set; }

    /// <summary>
    /// Admin Note
    /// </summary>
    public string? AdminFinalResult_Note { get; set; }

    public DateTime User_Creation_Date { get; set; }

    /// <summary>
    /// موقف الحجر
    /// </summary>
    public bool? Admin_Confirmation { get; set; }

    /// <summary>
    /// ادمن الحجر
    /// </summary>
    public short? Admin_User { get; set; }

    public DateTime? Admin_Date { get; set; }

    public double? Area_Acres { get; set; }

    public virtual Farm_Committee FarmCommittee { get; set; } = null!;

    public virtual ICollection<Farm_Committee_Examination_Confirm> Farm_Committee_Examination_Confirms { get; set; } = new List<Farm_Committee_Examination_Confirm>();

    public virtual Farm_Request_ItemCategory? Farm_Request_ItemCategories { get; set; }
}
