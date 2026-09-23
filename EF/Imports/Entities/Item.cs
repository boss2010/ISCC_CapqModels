using System;
using System.Collections.Generic;

namespace EF.Imports.Entities;

/// <summary>
/// النبات و المنتجات
/// </summary>
public partial class Item
{
    public long ID { get; set; }

    /// <summary>
    /// الاسم بالعربية
    /// </summary>
    public string? Name_Ar { get; set; }

    /// <summary>
    /// الاسم بالانجليزية
    /// </summary>
    public string? Name_En { get; set; }

    public string? Scientific_Name { get; set; }

    public int? Family_ID { get; set; }

    public int? Group_ID { get; set; }

    /// <summary>
    /// وصف أو تنويه
    /// </summary>
    public string? Descreption_Ar { get; set; }

    /// <summary>
    /// وصف أو تنويه
    /// </summary>
    public string? Descreption_En { get; set; }

    public string? Picture { get; set; }

    /// <summary>
    /// 0 مسموح به
    /// 1 ممنوع 
    /// 
    /// </summary>
    public bool IsForbidden { get; set; }

    public string? ForbiddenReason { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    /// <summary>
    /// هل له اذن استيراد -خاص بالوارد
    /// </summary>
    public bool? IsPermissionRequest { get; set; }

    public byte? Item_Type_ID { get; set; }

    /// <summary>
    /// معروف وغير معروف
    /// </summary>
    public bool? Is_known_item { get; set; }

    public bool? IsPlantInEgypt { get; set; }

    public string? Item_Code { get; set; }

    public bool? Agriculture_17 { get; set; }

    public string? HSCode { get; set; }

    public virtual Family? Family { get; set; }

    public virtual Group? Group { get; set; }

    public virtual ICollection<Im_CommitteeResult_Infection> Im_CommitteeResult_Infection { get; set; } = new List<Im_CommitteeResult_Infection>();

    public virtual ICollection<Item_ShortName> Item_ShortName { get; set; } = new List<Item_ShortName>();
}
