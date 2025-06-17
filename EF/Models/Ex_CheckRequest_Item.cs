using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Ex_CheckRequest_Item
{
    public long ID { get; set; }

    public int? SubPart_id { get; set; }

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

    public decimal? Net_Weight { get; set; }

    public decimal? Fees { get; set; }

    public long? Item_ShortName_ID { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public long? ItemCategory_ID { get; set; }

    public long? Ex_CheckRequest_ID { get; set; }

    public long? FarmsData_ID { get; set; }

    public short? Governate_ID { get; set; }

    public short? Center_ID { get; set; }

    public short? Village_ID { get; set; }

    /// <summary>
    /// ناحية الزراعة
    /// </summary>
    public string? Agriculture_Hand { get; set; }

    public string? Item_Permission_Number { get; set; }

    public long? ItemCategories_Group_ID { get; set; }

    public decimal? Net_WeightOld { get; set; }

    public virtual Center? Center { get; set; }

    public virtual Ex_CheckRequest? Ex_CheckRequest { get; set; }

    public virtual ICollection<Ex_CheckRequest_Items_Lot_Category> Ex_CheckRequest_Items_Lot_Categories { get; set; } = new List<Ex_CheckRequest_Items_Lot_Category>();

    public virtual FarmsDatum? FarmsData { get; set; }

    public virtual Governate? Governate { get; set; }

    public virtual ItemCategories_Group? ItemCategories_Group { get; set; }

    public virtual ItemCategory? ItemCategory { get; set; }

    public virtual Item_ShortName? Item_ShortName { get; set; }

    public virtual Village? Village { get; set; }
}
