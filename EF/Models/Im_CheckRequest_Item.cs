using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// نباتات طلب الفحص الوارد
/// </summary>
public partial class Im_CheckRequest_Item
{
    public long ID { get; set; }

    public long? Im_CheckRequset_Shipping_Method_ID { get; set; }

    public long? Im_Initiator_ID { get; set; }

    public int? SubPart_id { get; set; }

    public string? Item_Permission_Number { get; set; }

    public short? Package_Material_ID { get; set; }

    public short? Package_Type_ID { get; set; }

    public int? Package_Count { get; set; }

    public decimal? Package_Weight { get; set; }

    public int? Units_Number { get; set; }

    public bool? Is_LotDivision { get; set; }

    public double? Size { get; set; }

    public string? Order_Text { get; set; }

    public decimal? GrossWeight { get; set; }

    public bool IsAccepted { get; set; }

    public DateOnly? Accept_Date { get; set; }

    public DateTime? Accept_User_Updation_Date { get; set; }

    public short? Accept_User_Creation_Id { get; set; }

    public DateTime? Accept_User_Creation_Date { get; set; }

    public short? Accept_User_Updation_Id { get; set; }

    public short? Country_ID { get; set; }

    /// <summary>
    /// الوزن الصافي
    /// </summary>
    public decimal? Net_Weight { get; set; }

    public decimal? Fees { get; set; }

    public long? Item_ShortName_ID { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public decimal? Fees_Actual { get; set; }

    public bool? IsPaid { get; set; }

    public short? QualitativeGroup_Id { get; set; }

    public virtual ICollection<Im_CheckRequest_Items_Lot_Category> Im_CheckRequest_Items_Lot_Categories { get; set; } = new List<Im_CheckRequest_Items_Lot_Category>();

    public virtual Im_CheckRequset_Shipping_Method? Im_CheckRequset_Shipping_Method { get; set; }

    public virtual ICollection<Im_Execution_Item> Im_Execution_Items { get; set; } = new List<Im_Execution_Item>();

    public virtual Im_Initiator? Im_Initiator { get; set; }

    public virtual ICollection<Im_PermissionItem_Division_Custody> Im_PermissionItem_Division_Custodies { get; set; } = new List<Im_PermissionItem_Division_Custody>();
}
