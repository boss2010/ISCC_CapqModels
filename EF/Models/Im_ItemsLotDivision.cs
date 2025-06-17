using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// تفاصيل اللوط
/// </summary>
public partial class Im_ItemsLotDivision
{
    public long ID { get; set; }

    public long Im_PermissionItems_ID { get; set; }

    /// <summary>
    /// الوزن الصافي
    /// </summary>
    public decimal? Net_Weight { get; set; }

    /// <summary>
    /// رقم اللوط
    /// </summary>
    public string? Lot_Number { get; set; }

    /// <summary>
    /// مادة العبوة
    /// </summary>
    public short? Package_Material_ID { get; set; }

    /// <summary>
    /// نوع العبوة
    /// </summary>
    public short? Package_Type_ID { get; set; }

    /// <summary>
    /// عدد العبوات
    /// </summary>
    public int? Package_Count { get; set; }

    public long? Farm_ID { get; set; }

    /// <summary>
    /// وزن العبوة
    /// </summary>
    public decimal? Package_Weight { get; set; }

    /// <summary>
    /// الوزن القائم
    /// </summary>
    public decimal? Gross_Weight { get; set; }

    /// <summary>
    /// رقم الحاوية
    /// </summary>
    public string? Container_Number { get; set; }

    /// <summary>
    /// رقم السيل الملاحي
    /// </summary>
    public string? NavigationalFluid_Number { get; set; }

    /// <summary>
    /// رقم بوليصة الشحن
    /// </summary>
    public string? ShipmentPolicy_Number { get; set; }

    /// <summary>
    /// مقبول = 1 / مرفوض =0
    /// </summary>
    public bool? IsAccepted { get; set; }

    public string? RejectReason { get; set; }

    public virtual Im_PermissionItem Im_PermissionItems { get; set; } = null!;
}
