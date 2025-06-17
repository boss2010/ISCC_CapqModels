using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// الأصناف الزراعية
/// </summary>
public partial class ItemCategory
{
    public long ID { get; set; }

    public long? Item_ID { get; set; }

    public string? Name_Ar { get; set; }

    public string? Name_En { get; set; }

    /// <summary>
    /// الجهة الطالبة للتسجيل
    /// </summary>
    public long? Company_ID { get; set; }

    /// <summary>
    /// هل مسجل ام لا
    /// </summary>
    public bool? IsRegister { get; set; }

    public string? Register_NumDate { get; set; }

    public DateOnly? Register_EndDate { get; set; }

    /// <summary>
    /// نهاية المهلة
    /// </summary>
    public DateOnly? TimeOut { get; set; }

    /// <summary>
    ///  0 لو ممنوع 1 لو شغال
    /// </summary>
    public bool IsForbidden { get; set; }

    /// <summary>
    /// if under protection 1 / 0 if not (تحت الحماية أو لا)
    /// </summary>
    public bool CurrentStatus { get; set; }

    /// <summary>
    /// قرار حماية الماكية ملف Pdf
    /// </summary>
    public string? Protect_Property { get; set; }

    public string? Notes { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public int? Resolution_Number { get; set; }

    public short? ItemCategories_Type { get; set; }

    public bool? Is_Plant_Egypt { get; set; }

    public long? ItemCategories_Group_ID { get; set; }

    public int? Resolution_Date { get; set; }

    public virtual Company_National? Company { get; set; }

    public virtual ICollection<Ex_CheckRequest_Item> Ex_CheckRequest_Items { get; set; } = new List<Ex_CheckRequest_Item>();

    public virtual ICollection<Farm_ItemCategory> Farm_ItemCategories { get; set; } = new List<Farm_ItemCategory>();

    public virtual ICollection<Im_CheckRequest_Items_Lot_Category> Im_CheckRequest_Items_Lot_Categories { get; set; } = new List<Im_CheckRequest_Items_Lot_Category>();

    public virtual ICollection<Im_PermissionItems_Category> Im_PermissionItems_Categories { get; set; } = new List<Im_PermissionItems_Category>();

    public virtual Item? Item { get; set; }

    public virtual ItemCategories_Group? ItemCategories_Group { get; set; }

    public virtual ItemCategories_Type? ItemCategories_TypeNavigation { get; set; }
}
