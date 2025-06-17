using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Ex_CheckRequest_Items_Lot_Category
{
    public long ID { get; set; }

    public long? Ex_CheckRequest_Items_ID { get; set; }

    public short? Package_Material_ID { get; set; }

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
    /// الوزن العبوه الفارغ
    /// </summary>
    public decimal? Package_Weight { get; set; }

    /// <summary>
    /// وزن العبوة القائم
    /// </summary>
    public decimal? Package_Based_Weight { get; set; }

    /// <summary>
    /// وزن العبوة الصافي
    /// </summary>
    public decimal? Package_Net_Weight { get; set; }

    public int? Units_Number { get; set; }

    public double? Size { get; set; }

    public string? Order_Text { get; set; }

    public string? Reason_Entry { get; set; }

    public string? Lot_Number { get; set; }

    public bool? IsAccepted { get; set; }

    public string? RejectReason { get; set; }

    public string? Grower_Number { get; set; }

    public string? Waybill { get; set; }

    public string? Number_Wooden_Package { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public long? Ex_CheckRequset_Shipping_MethodID { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public long? User_Updation_Id { get; set; }

    public decimal? GrossWeightOld { get; set; }

    public virtual Ex_CheckRequest_Item? Ex_CheckRequest_Items { get; set; }

    public virtual ICollection<Ex_CheckRequest_Items_Lot_Result> Ex_CheckRequest_Items_Lot_Results { get; set; } = new List<Ex_CheckRequest_Items_Lot_Result>();

    public virtual Ex_CheckRequset_Shipping_Method? Ex_CheckRequset_Shipping_Method { get; set; }
}
