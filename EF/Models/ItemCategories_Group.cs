using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// المجموعة الصنفية
/// </summary>
public partial class ItemCategories_Group
{
    public long ID { get; set; }

    public string? Name_En { get; set; }

    public string? Name_Ar { get; set; }

    public string? Descreption_En { get; set; }

    public string? Descreption_Ar { get; set; }

    public long? Item_ID { get; set; }

    public bool IsActive { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public virtual ICollection<Ex_CheckRequest_Item> Ex_CheckRequest_Items { get; set; } = new List<Ex_CheckRequest_Item>();

    public virtual ICollection<Im_PermissionItems_Category> Im_PermissionItems_Categories { get; set; } = new List<Im_PermissionItems_Category>();

    public virtual ICollection<ItemCategory> ItemCategories { get; set; } = new List<ItemCategory>();
}
