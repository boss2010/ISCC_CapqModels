using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// سلالة البند الخاضع
/// </summary>
public partial class ItemCategories_Type
{
    public short ID { get; set; }

    public string? Name_Ar { get; set; }

    public string? Name_En { get; set; }

    public bool IsActive { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public virtual ICollection<Im_ScientificResearch_ItemPlant_Inseket_Lieble> Im_ScientificResearch_ItemPlant_Inseket_Liebles { get; set; } = new List<Im_ScientificResearch_ItemPlant_Inseket_Lieble>();

    public virtual ICollection<ItemCategory> ItemCategories { get; set; } = new List<ItemCategory>();

    public virtual ICollection<LiableItem> LiableItems { get; set; } = new List<LiableItem>();
}
