using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Im_PermissionItems_Category
{
    public long ID { get; set; }

    public long? Im_PermissionItems_ID { get; set; }

    public long? ItemCategory_ID { get; set; }

    public long? ItemCategoryGroup_ID { get; set; }

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

    /// <summary>
    /// وزن العبوة
    /// </summary>
    public decimal? Package_Weight { get; set; }

    /// <summary>
    /// عدد الوحدات
    /// </summary>
    public int? Units_Number { get; set; }

    /// <summary>
    /// حجم الرسالة
    /// </summary>
    public double? Size { get; set; }

    /// <summary>
    /// الرتبة
    /// </summary>
    public string? Order_Text { get; set; }

    /// <summary>
    /// الوزن الاجمالى
    /// </summary>
    public decimal? GrossWeight { get; set; }

    /// <summary>
    /// سبب الدخول
    /// </summary>
    public string? Reason_Entry { get; set; }

    public virtual Im_PermissionItem? Im_PermissionItems { get; set; }

    public virtual ItemCategory? ItemCategory { get; set; }

    public virtual ItemCategories_Group? ItemCategoryGroup { get; set; }
}
