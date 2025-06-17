using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// منافستو
/// </summary>
public partial class Im_Manafest
{
    public long ID { get; set; }

    /// <summary>
    /// رقم المنافيست
    /// </summary>
    public string Manafest_Num { get; set; } = null!;

    /// <summary>
    /// تاريخ تقديم المنافيست
    /// </summary>
    public DateOnly? SubmissionDate { get; set; }

    /// <summary>
    /// اسم الباخرة
    /// </summary>
    public string? ShipName { get; set; }

    /// <summary>
    /// تاريخ الوصول
    /// </summary>
    public DateOnly? ArriveDate { get; set; }

    /// <summary>
    /// وقت الوصول
    /// </summary>
    public TimeOnly? ArriveTime { get; set; }

    /// <summary>
    /// شركة الملاحة
    /// </summary>
    public string? NavigationCompany { get; set; }

    /// <summary>
    /// المنشأ(دولة)
    /// </summary>
    public string? Origin { get; set; }

    /// <summary>
    /// ميناء الشحن
    /// </summary>
    public string? ShipmentPort { get; set; }

    /// <summary>
    /// رقم البوليصة
    /// </summary>
    public string? PolicyNumber { get; set; }

    /// <summary>
    /// اسم المستورد
    /// </summary>
    public string? ImporterName { get; set; }

    /// <summary>
    /// اسم الصنف
    /// </summary>
    public string? PlantName { get; set; }

    /// <summary>
    /// العدد
    /// </summary>
    public int? Quantity { get; set; }

    /// <summary>
    /// الوحدة
    /// </summary>
    public string? Unit { get; set; }

    /// <summary>
    /// الوزن القائم
    /// </summary>
    public decimal? GrossWeight { get; set; }

    /// <summary>
    /// الوزن الصافي
    /// </summary>
    public decimal? Net_Weight { get; set; }

    /// <summary>
    /// بيان التعديلات
    /// </summary>
    public string? EditRecord { get; set; }

    /// <summary>
    /// تاريخ نهاية التفريغ
    /// </summary>
    public DateOnly? DischargeEndDate { get; set; }

    /// <summary>
    /// تاريخ التقدم للمنافيستو للحجر
    /// </summary>
    public DateOnly? ToHagrDate { get; set; }

    /// <summary>
    /// تاريخ تسديد الرسالة
    /// </summary>
    public DateOnly? ExaminationDate { get; set; }

    /// <summary>
    /// رقم الشهادة الجمركية
    /// </summary>
    public string? CustomsCertificate { get; set; }

    /// <summary>
    /// رقم طلب الاتمام
    /// </summary>
    public string? CompletionApplicationNum { get; set; }

    /// <summary>
    /// ترانزيت أم لا
    /// </summary>
    public bool IsTransit { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public virtual ICollection<Im_CheckRequest_Manafest> Im_CheckRequest_Manafests { get; set; } = new List<Im_CheckRequest_Manafest>();
}
