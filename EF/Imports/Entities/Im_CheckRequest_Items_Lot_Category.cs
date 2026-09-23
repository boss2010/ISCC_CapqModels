using System;
using System.Collections.Generic;

namespace EF.Imports.Entities;

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

    /// <summary>
    /// علامة مميزة
    /// </summary>
    public string? DistinctiveMark { get; set; }

    /// <summary>
    /// عدد الطرود
    /// </summary>
    public string? PackagesCount { get; set; }

    /// <summary>
    /// علامة مميزة
    /// </summary>
    public string? ShipName { get; set; }

    /// <summary>
    /// عبوات او بدون
    /// </summary>
    public int? containers_type_ID { get; set; }

    /// <summary>
    /// تاريخ الرحلة
    /// </summary>
    public DateOnly? TripDate { get; set; }

    public virtual Im_CheckRequest_Items? Im_CheckRequest_Items { get; set; }
}
