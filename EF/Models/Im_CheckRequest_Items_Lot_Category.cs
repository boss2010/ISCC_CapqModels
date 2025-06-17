using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// تقسيم لوط وااصناف
/// </summary>
public partial class Im_CheckRequest_Items_Lot_Category
{
    public long ID { get; set; }

    public long? Im_CheckRequest_Items_ID { get; set; }

    public long? ItemCategory_ID { get; set; }

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
    /// اجمالى الوزن القائم لللوطات
    /// </summary>
    public decimal? GrossWeight { get; set; }

    /// <summary>
    /// الوزن الصافي لللوطات
    /// </summary>
    public decimal? Net_Weight { get; set; }

    /// <summary>
    /// مش مستخدم
    /// </summary>
    public decimal? Based_Weight { get; set; }

    /// <summary>
    /// الوزن العبوه الصافي
    /// </summary>
    public decimal? Package_Weight { get; set; }

    /// <summary>
    /// وزن العبوة القائم
    /// </summary>
    public decimal? Package_Based_Weight { get; set; }

    /// <summary>
    /// وزن العبوة الفارغ
    /// </summary>
    public decimal? Package_Net_Weight { get; set; }

    /// <summary>
    /// عدد الوحدات
    /// </summary>
    public int? Units_Number { get; set; }

    public double? Size { get; set; }

    public string? Order_Text { get; set; }

    /// <summary>
    /// سبب الدخول
    /// </summary>
    public string? Reason_Entry { get; set; }

    /// <summary>
    /// رقم اللوط
    /// </summary>
    public string? Lot_Number { get; set; }

    /// <summary>
    /// مقبول = 1 / مرفوض =0
    /// </summary>
    public bool? IsAccepted { get; set; }

    /// <summary>
    /// اسباب الرفض
    /// </summary>
    public string? RejectReason { get; set; }

    /// <summary>
    /// رقم المزرعه من الصادر ,وبعض الحالات من الوارد
    /// </summary>
    public string? Grower_Number { get; set; }

    /// <summary>
    /// رقم بوليصه الشحن
    /// </summary>
    public string? Waybill { get; set; }

    /// <summary>
    /// عدد وحدات التعبئه الخشبيه 
    /// </summary>
    public string? Number_Wooden_Package { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public virtual Im_CheckRequest_Item? Im_CheckRequest_Items { get; set; }

    public virtual ICollection<Im_Execution_Item> Im_Execution_Items { get; set; } = new List<Im_Execution_Item>();

    public virtual ItemCategory? ItemCategory { get; set; }
}
